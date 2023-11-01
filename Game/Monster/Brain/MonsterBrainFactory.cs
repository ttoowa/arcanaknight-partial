namespace ArcaneSurvivorsClient.Game {
    public static class MonsterBrainFactory {
        public static IPawnBrain CreateBrain(MonsterBrainType type) {
            switch (type) {
                default:
                    throw LogBuilder.BuildException(LogType.Error, nameof(MonsterBrainFactory),
                        "Invalid MonsterBrainType.", new LogElement("MonsterBrainType", type.ToString()));
                case MonsterBrainType.Walker:
                    return new MonsterBrain_Walker();
                case MonsterBrainType.Sprinter:
                    return new MonsterBrain_Sprinter();
                case MonsterBrainType.Drunker:
                    return new MonsterBrain_Drunker();
                case MonsterBrainType.Bow:
                    return new MonsterBrain_Bow();
            }
        }
    }
}