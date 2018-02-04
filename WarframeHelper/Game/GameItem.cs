using System.Collections.Generic;

namespace WarframeHelper.Game
{
    public struct GameItem
    {
        public static GameItem[] ParseAllItems()
        {
            var items = new List<GameItem>();

            var warframes = Warframe.Parse(@"Data\warframes.json");
            foreach (var warframe in warframes)
            {
                GameItem item = new GameItem
                {
                    name = warframe.name,
                    item = warframe,
                    type = ItemType.Warframe
                };
                items.Add(item);
            }

            var weapons = Weapon.Parse(@"Data\weapons.json");
            foreach (var weapon in weapons)
            {
                GameItem item = new GameItem
                {
                    name = weapon.name,
                    item = weapon,
                    type = ItemType.Weapon
                };
                items.Add(item);
            }

            var arcanes = Arcane.Parse(@"Data\arcanes.json");
            foreach (var arcane in arcanes)
            {
                GameItem item = new GameItem()
                {
                    name = arcane.name,
                    item = arcane,
                    type = ItemType.Arcane
                };
                items.Add(item);
            }

            return items.ToArray();
        }

        public string name { get; set; }
        public object item { get; set; }
        public ItemType type { get; set; }
    }

    public enum ItemType { Warframe, Weapon, Arcane }
}