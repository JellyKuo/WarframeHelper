using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                GameItem item = new GameItem();
                item.name = warframe.name;
                item.item = warframe;
                item.type = ItemType.Warframe;
                items.Add(item);
            }

            var weapons = Weapon.Parse(@"Data\weapons.json");
            foreach (var weapon in weapons)
            {
                GameItem item = new GameItem();
                item.name = weapon.name;
                item.item = weapon;
                item.type = ItemType.Weapon;
                items.Add(item);
            }

            return items.ToArray();
        }

        public string name { get; set; }
        public object item { get; set; }
        public ItemType type { get; set; }
    }

    public enum ItemType { Warframe, Weapon,Arcane }
}
