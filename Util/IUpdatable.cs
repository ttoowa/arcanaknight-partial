namespace ArcaneSurvivorsClient {
    public interface IUpdatable {
        public bool IsActive { get; set; }

        public void OnTick(float deltaTime);
    }
}