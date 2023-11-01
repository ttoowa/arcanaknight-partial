using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using Google.Play.Common;
using Google.Play.Review;

#elif UNITY_IOS
using UnityEngine.iOS;
#endif

namespace ArcaneSurvivorsClient {
    public static class MobilePlatformManager {
        public static bool IsSaving { get; private set; }
        public static bool IsLoggedIn { get; set; }

        static MobilePlatformManager() {
            PlayGamesPlatform.Activate();
        }

        // Auth
        public static async Task<bool> AuthenticateAccount() {
            bool taskComplete = false;
            PlayGamesPlatform.Instance.Authenticate((SignInStatus status) => {
                if (IsLoggedIn) return;

                Debug.Log($"[GooglePlay] Authenticated: {status}");
                if (status == SignInStatus.Success)
                    IsLoggedIn = true;
                else
                    IsLoggedIn = false;

                taskComplete = true;
            });

            while (!taskComplete) {
                await Task.Delay(100);
            }

            return IsLoggedIn;
        }

        // Save & Load
        [ItemCanBeNull]
        public static async Task<ISavedGameMetadata> OpenOrCreateGame() {
            bool isComplete = false;
            ISavedGameMetadata metadata = null;

            // TODO: Make the filename configurable.
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution("Default", DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseMostRecentlySaved, (status, _metadata) => {
                    if (status == SavedGameRequestStatus.Success)
                        metadata = _metadata;
                    isComplete = true;
                });

            while (!isComplete) {
                await Task.Delay(100);
            }

            return metadata;
        }

        public static async Task<string> LoadGame() {
            ISavedGameMetadata metadata = await OpenOrCreateGame();

            if (metadata == null) return null;

            bool isComplete = false;
            byte[] data = null;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(metadata, (status, _data) => {
                if (status == SavedGameRequestStatus.Success)
                    data = _data;

                Debug.Log($"[GooglePlay] LoadGame: {status}");
                isComplete = true;
            });

            while (!isComplete) {
                await Task.Delay(100);
            }

            if (data == null) return null;

            return Encoding.UTF8.GetString(data);
        }

        public static async Task<bool> SaveGame(string data) {
            if (IsSaving)
                return false;

            try {
                IsSaving = true;

                byte[] rawData = Encoding.UTF8.GetBytes(data);

                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                ISavedGameMetadata metadata = await OpenOrCreateGame();

                if (metadata == null) return false;

                SavedGameMetadataUpdate.Builder builder = new();
                SavedGameMetadataUpdate updatedMetadata = builder.Build();

                bool isSuccess = false;
                bool isComplete = false;

                savedGameClient.CommitUpdate(metadata, updatedMetadata, rawData, (status, _) => {
                    isSuccess = status == SavedGameRequestStatus.Success;
                    isComplete = true;
                    Debug.Log($"[GooglePlay] SaveGame: {status}");
                });

                while (!isComplete) {
                    await Task.Delay(100);
                }

                return isSuccess;
            } catch {
                return false;
            } finally {
                IsSaving = false;
            }
        }

        public static async void DeleteGame() {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            ISavedGameMetadata metadata = await OpenOrCreateGame();

            if (metadata == null) return;

            savedGameClient.Delete(metadata);
        }

        // Utils
        public static void FloatReview(bool onlyFirstTime) {
            string floatedStateKey = "StoreReviewRequestFloated";
            if (onlyFirstTime) {
                int reviewFloated = SaveData.LoadInteger(floatedStateKey, 0);

                if (reviewFloated == 1)
                    return;
            }

            SaveData.SaveInteger(floatedStateKey, 1);

#if UNITY_ANDROID
            ReviewManager reviewManager = new();

            // Start preloading the review prompt in the background
            PlayAsyncOperation<PlayReviewInfo, ReviewErrorCode> playReviewInfoAsyncOperation =
                reviewManager.RequestReviewFlow();

            // Define a callback after the preloading is done
            playReviewInfoAsyncOperation.Completed += playReviewInfoAsync => {
                if (playReviewInfoAsync.Error == ReviewErrorCode.NoError) {
                    // Display the review prompt
                    PlayReviewInfo playReviewInfo = playReviewInfoAsync.GetResult();
                    reviewManager.LaunchReviewFlow(playReviewInfo);

                    LogBuilder.Log(LogType.Info, "GooglePlayUtility.FloatReview", $"Review popup floated.");
                } else {
                    // Handle error when loading review prompt
                    LogBuilder.Log(LogType.Warning, "GooglePlayUtility.FloatReview", $"Failed to load review prompt.",
                        new[] { new LogElement("ErrorCode", playReviewInfoAsync.Error.ToString()) });
                }
            };
#elif UNITY_IOS
            Device.RequestStoreReview();
#endif
        }
    }
}