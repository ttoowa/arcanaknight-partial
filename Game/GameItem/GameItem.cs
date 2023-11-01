using ArcaneSurvivorsClient.Locale;

namespace ArcaneSurvivorsClient.Game {
    public class GameItem {
        public GameItemType itemType;
        [LocaleKey] public string name;
        [LocaleKey] public string description;
        public int maxStack = 1;
    }
}