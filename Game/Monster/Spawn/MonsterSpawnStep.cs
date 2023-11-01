namespace ArcaneSurvivorsClient.Game {
    public struct MonsterSpawnStep {
        public float time;
        public MonsterSpawnSet spawnSet;

        public float HpSum => spawnSet.HpSum;

        public MonsterSpawnStep(float time, MonsterSpawnSet spawnSet) {
            this.time = time;
            this.spawnSet = spawnSet;
        }

        public override string ToString() {
            return $"[{time.ToString("0.0")} sec] {spawnSet}";
        }
    }
}