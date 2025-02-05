using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG
{
    abstract class Screen
    {
        public abstract void DrawFuncScreen();
    }

    class StatusScreen : Screen
    {
        private static StatusScreen instance = new StatusScreen();

        private StatusScreen() { }

        public static StatusScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StatusScreen();
                }
                return instance;
            }
        }

        public override void DrawFuncScreen()
        {
            Player player = Player.Instance;
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"공격력 : {player.TAtk}");
            Console.WriteLine($"방어력 : {player.TDef}");
            Console.WriteLine($"체 력: {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
        }
    }

    class ShopScreen : Screen
    {
        private static ShopScreen instance = new ShopScreen();

        private ShopScreen() { }

        public static ShopScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShopScreen();
                }
                return instance;
            }
        }

        public override void DrawFuncScreen()
        {
            int index = 1;
            Console.WriteLine("[아이템 목록]");
            foreach (Item _Item in GameManager.Instance.ShopItem.Values)
            {
                string EMark = _Item.Isequip ? "[E] " : "";
                string Stat = "";
                string Price = "";
                switch (_Item.EEP)
                {
                    case EEquipPart.Weapon:
                        Stat = $"공격력 +{_Item.Atk}";
                        break;
                    case EEquipPart.Armor:
                        Stat = $"방어력 +{_Item.Def}";
                        break;
                }

                if (Player.Instance.EquipmentItems.Contains(_Item.Name))
                {
                    Price = "판매 완료";
                }
                else
                {
                    Price = $"{_Item.Price} G";
                }

                if (EScreen.Shop == GameManager.Instance.NowScreen)
                {
                    Console.WriteLine($"- {EMark}{_Item.Name}  | {Stat} | {_Item.Explain} | {Price}");
                }
                else if (EScreen.Buy == GameManager.Instance.NowScreen)
                {
                    Console.WriteLine($"- {index} {EMark}{_Item.Name}  | {Stat} | {_Item.Explain} | {Price}");
                    index++;
                }
            }
            Console.WriteLine("\n");
        }
    }

    class InvemtoryScreen : Screen
    {
        private static InvemtoryScreen instance = new InvemtoryScreen();

        private InvemtoryScreen() { }

        public static InvemtoryScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvemtoryScreen();
                }
                return instance;
            }
        }

        public override void DrawFuncScreen()
        {
            int index = 1;
            Console.WriteLine("[아이템 목록]");
            foreach (Item _Item in Player.Instance.EquipmentItems.Values)
            {
                string EMark = _Item.Isequip ? "[E] " : "";
                string Stat = "";
                switch (_Item.EEP)
                {
                    case EEquipPart.Weapon:
                        Stat = $"공격력 +{_Item.Atk}";
                        break;
                    case EEquipPart.Armor:
                        Stat = $"방어력 +{_Item.Def}";
                        break;

                }

                if (EScreen.Inventory == GameManager.Instance.NowScreen)
                {
                    Console.WriteLine($"- {EMark}{_Item.Name}  | {Stat} | {_Item.Explain}");
                }
                else if (EScreen.Equip == GameManager.Instance.NowScreen)
                {
                    Console.WriteLine($"- {index} {EMark}{_Item.Name}  | {Stat} | {_Item.Explain}");
                    index++;
                }
                else if (EScreen.Sell == GameManager.Instance.NowScreen)
                {
                    Console.WriteLine($"- {index} {EMark}{_Item.Name}  | {Stat} | {_Item.Explain}");
                    index++;
                }
            }
            Console.WriteLine("\n");
        }
    }

    class DungeonScreen : Screen
    {
        private static DungeonScreen instance = new DungeonScreen();

        private DungeonScreen() { }

        public static DungeonScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DungeonScreen();
                }
                return instance;
            }
        }

        public override void DrawFuncScreen()
        {
            if (true == GameManager.Instance.IsClear)
            {
                Console.WriteLine("클리어\n축하합니다!!");
                string DName = "";
                switch (GameManager.Instance.DChoice)
                {
                    case 1:
                        DName = "쉬운 던전";
                        break;
                    case 2:
                        DName = "일반 던전";
                        break;
                    case 3:
                        DName = "어려운 던전";
                        break;
                }
                Console.WriteLine($"{DName}을 클리어하였습니다.\n");
                Console.WriteLine($"[탐험 결과]\n체력 {Player.Instance.Hp} -> {GameManager.Instance.DHp}\nGold {Player.Instance.Gold} -> {GameManager.Instance.DGold}");
                Player.Instance.Hp = GameManager.Instance.DHp;
                Player.Instance.Gold = GameManager.Instance.DGold;

                Player.Instance.DClearStack++;
                Player.Instance.LevelupCheck();
            }
            else
            {
                Console.WriteLine("클리어\n실패...\n");
                Console.WriteLine($"[탐험 결과]\n체력 {Player.Instance.Hp} -> {GameManager.Instance.DHp}");
                Player.Instance.Hp = GameManager.Instance.DHp;
            }
        }
    }
}
