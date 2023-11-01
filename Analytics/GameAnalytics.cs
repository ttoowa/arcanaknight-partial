using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;

namespace ArcaneSurvivorsClient.Analytics {
    public static class GameAnalytics {
        public static bool AnalyticsEnabled { get; private set; }

        public static async Task Init() {
            bool isComplete = false;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                if (task.Result == DependencyStatus.Available)
                    AnalyticsEnabled = true;
                else
                    AnalyticsEnabled = false;

                isComplete = true;
            });

            while (!isComplete) {
                await Task.Delay(100);
            }
        }

        public static bool LogEvent(string name, params Parameter[] parameters) {
            if (!AnalyticsEnabled) return false;

            try {
                if (parameters == null || parameters.Length == 0)
                    FirebaseAnalytics.LogEvent(name);
                else
                    FirebaseAnalytics.LogEvent(name, parameters);

                return true;
            } catch {
                return false;
            }
        }
    }
}