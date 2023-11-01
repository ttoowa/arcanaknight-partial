using System;
using System.Collections.Generic;

namespace ArcaneSurvivorsClient.Game {
    public static class GameItemLibrary {
        public static readonly GameItem[] Items = new[] {
            new GameItem {
                itemType = GameItemType.Stick,
                name = "Stick",
                description = "Just a stick.",
                maxStack = 1
            },
            new GameItem {
                itemType = GameItemType.Dagger,
                name = "Dagger",
                description = "A dagger.",
                maxStack = 1
            },
            new GameItem {
                itemType = GameItemType.Sword,
                name = "Sword",
                description = "Sharp sword.",
                maxStack = 1
            }
        };

        private static readonly Dictionary<GameItemType, GameItem> ItemDict;

        static GameItemLibrary() {
            ItemDict = new Dictionary<GameItemType, GameItem>();
            foreach (GameItem item in Items) {
                if (ItemDict.ContainsKey(item.itemType)) {
                    throw LogBuilder.BuildException(LogType.Error, "GameItemLibrary",
                        "Item with same ItemType already exists.",
                        new LogElement("ItemType", item.itemType.ToString()));
                }


                ItemDict.Add(item.itemType, item);
            }
        }

        public static GameItem GetItem(GameItemType itemType) {
            if (!ItemDict.ContainsKey(itemType))
                return null;

            return ItemDict[itemType];
        }
    }
}