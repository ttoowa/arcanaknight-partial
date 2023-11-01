namespace ArcaneSurvivorsClient.Locale {
    public static class LocaleUtility {
        public static string ToLocale(this string localeKey, params string[] parameters) {
            return LocaleManager.GetString(localeKey, parameters);
        }
    }
}