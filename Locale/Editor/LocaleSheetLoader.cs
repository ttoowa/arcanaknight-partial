using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace ArcaneSurvivorsClient.Locale {
    public class LocaleSheetLoader : MonoBehaviour {
        [MenuItem("Locale/LoadLocaleSheet")]
        public static void LoadLocaleSheet() {
            // Load data from GoogleSheet
            // 로컬시트 ID
            // https://docs.google.com/spreadsheets/d/1NmJ4MYJONJRobgh3YcdpTlRefPReIKE1h6ljScd5oDQ/edit#gid=0
            string spreadsheetId = "1NmJ4MYJONJRobgh3YcdpTlRefPReIKE1h6ljScd5oDQ";

            GoogleSheetClient sheetClient = new(spreadsheetId);

            Spreadsheet spreadsheet = sheetClient.GetSpreadsheet();
            Sheet sheet = spreadsheet.Sheets.First();

            IList<IList<object>> rows = sheetClient.GetCellValues(sheet.Properties.Title).Values;

            Dictionary<string, LocaleSheet> localeSheetDict = new();

            // Read cells : Collect words
            int keyColumn = -1;
            int dataColumn = -1;
            List<LanguageCell> languageList = new();
            for (int columnI = 0; columnI < rows[0].Count; ++columnI) {
                string content = rows[0][columnI].ToString();
                if (content.ToLower() == "key") {
                    keyColumn = columnI;
                    dataColumn = columnI + 1;
                } else if (dataColumn > 0) {
                    if (columnI >= dataColumn && !string.IsNullOrWhiteSpace(content))
                        languageList.Add(new LanguageCell(content, columnI));

                    localeSheetDict.Add(content, new LocaleSheet(content));
                }
            }

            for (int rowI = 1; rowI < rows.Count; ++rowI) {
                IList<object> row = rows[rowI];

                if (row.Count <= keyColumn) continue;

                string key = row[keyColumn].ToString();
                foreach (LanguageCell languageCell in languageList) {
                    if (row.Count <= languageCell.column)
                        continue;

                    string content = row[languageCell.column].ToString();

                    LocaleSheet localeSheet = localeSheetDict[languageCell.language];
                    localeSheet.Add(key, content);
                }
            }

            // Save to Json
            JObject jLocaleFile = new();
            foreach (LocaleSheet localeSheet in localeSheetDict.Values) {
                JObject jLocaleSheet = new();
                jLocaleFile.Add(localeSheet.Language, jLocaleSheet);
                foreach (KeyValuePair<string, string> wordPair in localeSheet) {
                    jLocaleSheet.Add(wordPair.Key, wordPair.Value);
                }
            }

            string localeSheetFilename = "Assets/Script/Locale/LocaleSheet.json";
            string jsonContent = JsonConvert.SerializeObject(jLocaleFile, Formatting.Indented);
            using (StreamWriter streamWriter = new(localeSheetFilename, false, Encoding.UTF8)) {
                streamWriter.Write(jsonContent);
            }

            UnityEditorUtility.ShowNotification("Loaded Locale Sheet successfully", true);
        }
    }


    public struct LanguageCell {
        public string language;
        public int column;

        public LanguageCell(string language, int column) {
            this.language = language;
            this.column = column;
        }
    }
}