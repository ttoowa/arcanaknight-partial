namespace ArcaneSurvivorsClient {
    public struct RangeF {
        public float Distance => max - min;

        public float min;
        public float max;

        public RangeF(float min, float max) {
            this.min = min;
            this.max = max;
        }

        public float Sample(float t) {
            return min + Distance * t;
        }
    }
}