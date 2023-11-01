namespace ArcaneSurvivorsClient {
    public class DevelopSetting {
        public const bool IsDevelopMode
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            = true;
#else
            = false;
#endif
    }
}