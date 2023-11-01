using System;

namespace ArcaneSurvivorsClient.Game {
    public class GameItemBundle {
        public string id;
        public GameItem item;
        public int amount;

        public Action AmountChanged;

        public GameItemBundle(GameItem item, int initAmount = 1) {
            id = Guid.NewGuid().ToString();
            this.item = item;
            amount = initAmount;
        }

        public GameItemBundle Clone() {
            return new GameItemBundle(item, amount);
        }

        public void AddAmount(int amount) {
            this.amount += amount;

            AmountChanged?.Invoke();
        }

        public void SetAmount(int amount) {
            this.amount = amount;

            AmountChanged?.Invoke();
        }
    }
}