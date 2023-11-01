namespace ArcaneSurvivorsClient {
    public class SimpleTimer {
        public float targetTime;
        public float elapsedTime;

        public SimpleTimer(float targetTime) {
            this.targetTime = targetTime;
        }

        public void Reset() {
            elapsedTime = 0f;
        }

        public bool Tick(float deltaTime) {
            elapsedTime += deltaTime;
            if (elapsedTime >= targetTime) {
                elapsedTime -= targetTime;
                return true;
            }

            return false;
        }
    }
}