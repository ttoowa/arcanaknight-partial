using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace ArcaneSurvivorsClient.Locale {
    public class GoogleSheetClient : IDisposable {
        public static string SecretFilename = "Auth/client-secret.json";
        public static string CredentialDirectory = "Auth/Credentials";
        public static string AppName = "ArcaneSurvivorsClient";

        public static int LastColumnIndex = 1000;
        public static int LastRowIndex = 100000;


        public SheetsService service;

        public string SpreadsheetId { get; }

        public GoogleSheetClient(string spreadsheetId) {
            SpreadsheetId = spreadsheetId;

            UserCredential credential;

            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromFile(SecretFilename).Secrets, new string[] {
                    SheetsService.Scope.Spreadsheets
                }, "user", CancellationToken.None, new FileDataStore(CredentialDirectory, true)).Result;

            service = new SheetsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }

        public void Dispose() {
            service?.Dispose();
        }

        // Spreadsheet management
        public Spreadsheet GetSpreadsheet() {
            return service.Spreadsheets.Get(SpreadsheetId).Execute();
        }

        // Sheet management
        public void ResetSpreadsheet() {
            Spreadsheet spreadsheet = GetSpreadsheet();

            // 첫번째 시트 비우기
            ClearSheetRequest(spreadsheet.Sheets.First().Properties.Title).Execute();

            // 나머지 시트 삭제하기
            List<Request> deleteRequestList = new();
            for (int i = 1; i < spreadsheet.Sheets.Count; ++i) {
                deleteRequestList.Add(DeleteSheetRequest(spreadsheet.Sheets[i].Properties.SheetId.Value));
            }

            BatchUpdateRequest(deleteRequestList.ToArray())?.Execute();
        }

        public Request CreateSheetRequest(string sheetName) {
            Request request = new() {
                AddSheet = new AddSheetRequest {
                    Properties = new SheetProperties {
                        Title = sheetName
                    }
                }
            };
            return request;
        }

        public Request DeleteSheetRequest(int sheetId) {
            Request request = new() {
                DeleteSheet = new DeleteSheetRequest {
                    SheetId = sheetId
                }
            };
            return request;
        }

        public Request RenameSheetRequest(int sheetId, string name) {
            Request request = new() {
                UpdateSheetProperties = new UpdateSheetPropertiesRequest {
                    Properties = new SheetProperties() {
                        Title = name,
                        SheetId = sheetId
                    },
                    Fields = "Title"
                }
            };
            return request;
        }

        /// <param name="range">The [A1 notation or R1C1 notation](/sheets/api/guides/concepts#cell) of the values to clear.</param>
        public SpreadsheetsResource.ValuesResource.ClearRequest ClearSheetRequest(string range) {
            return service.Spreadsheets.Values.Clear(new ClearValuesRequest(), SpreadsheetId, range);
        }

        public SpreadsheetsResource.BatchUpdateRequest BatchUpdateRequest(params Request[] requests) {
            if (requests == null || requests.Length == 0)
                return null;

            BatchUpdateSpreadsheetRequest batchUpdate = new() {
                Requests = requests
            };
            return service.Spreadsheets.BatchUpdate(batchUpdate, SpreadsheetId);
        }

        // Sheet values management
        /// <param name="range">The [A1 notation or R1C1 notation](/sheets/api/guides/concepts#cell) of the values to update.</param>
        public SpreadsheetsResource.ValuesResource.UpdateRequest UpdateValuesRequest(string range,
            IList<IList<object>> values) {
            ValueRange valueRange = new() {
                MajorDimension = "ROWS",
                Values = values
            };

            SpreadsheetsResource.ValuesResource.UpdateRequest request =
                service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            return request;
        }

        public SpreadsheetsResource.ValuesResource.BatchUpdateRequest BatchUpdateValuesRequest(
            params ValueRange[] valueRanges) {
            BatchUpdateValuesRequest batchUpdate = new() {
                ValueInputOption = "RAW",
                Data = valueRanges
            };
            return service.Spreadsheets.Values.BatchUpdate(batchUpdate, SpreadsheetId);
        }

        public ValueRange GetCellValues(string range) {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(SpreadsheetId, range);
            request.ValueRenderOption =
                SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.FORMATTEDVALUE;
            request.DateTimeRenderOption =
                SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum.SERIALNUMBER;

            return request.Execute();
        }

        // Extra functions
        public Request[] SetDataValidationRequest(GridRange gridRange, IList<string> conditionValues,
            string warnMessage = null) {
            List<ConditionValue> conditionValueList = conditionValues.Select(value => new ConditionValue() {
                UserEnteredValue = value
            }).ToList();
            DataValidationRule rule = new() {
                Condition = new BooleanCondition() {
                    Type = "ONE_OF_LIST",
                    Values = conditionValueList
                },
                InputMessage = warnMessage,
                ShowCustomUi = true,
                Strict = true
            };
            rule.ShowCustomUi = false;

            Request[] requests = new[] {
                // Data validation
                new Request() {
                    SetDataValidation = new SetDataValidationRequest {
                        Range = gridRange,
                        Rule = rule
                    }
                }
                // Style format
                // new Request() {
                //     AddConditionalFormatRule = new AddConditionalFormatRuleRequest {
                //         Rule = new ConditionalFormatRule {
                //             GradientRule = gradientRule,
                //             Ranges = new GridRange[] { gridRange }
                //         },
                //         Index = 0
                //     }
                // }
            };
            return requests;
        }

        public Request SetCellFormat(GridRange range, Color foreColor, Color backColor, bool isBold, bool isItalic) {
            CellFormat format = new() {
                BackgroundColor = backColor,
                TextFormat = new TextFormat() {
                    Bold = isBold,
                    Italic = isItalic,
                    ForegroundColor = foreColor
                }
            };

            Request request = new() {
                RepeatCell = new RepeatCellRequest() {
                    Range = range,
                    Fields = "UserEnteredFormat(backgroundColor,textFormat)",
                    Cell = new CellData() {
                        UserEnteredFormat = format
                    }
                }
            };

            return request;
        }

        public CellFormat GetCellFormat(string sheetName, string range) {
            SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(SpreadsheetId);
            request.Fields = "sheets(data(rowData(values(userEnteredFormat))))";
            request.Ranges = range;
            Spreadsheet sheet = request.Execute();

            CellData cellData = sheet.Sheets[0].Data[0].RowData[0].Values[0];
            return cellData.UserEnteredFormat;
        }
    }
}