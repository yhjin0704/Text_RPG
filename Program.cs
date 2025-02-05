using System;
using System.ComponentModel;
using System.Numerics;

namespace Text_RPG
{
    enum EScreen
    {
        Main,
        Status,
        Inventory,
        Equip,
        Shop,
        Buy,
        Sell,
        DStage,
        DClear,
        Rest,
    }

    internal class Program
    {
        static void Main(string[] _Args)
        {
            GameManager gameManager = GameManager.Instance;
            ScreenManager screenManager = ScreenManager.Instance;
            Player player = Player.Instance;

            AddShop();
            gameManager.NowScreen = EScreen.Main;
            int choice;

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            NameSetting();
            JobSetting();

            while (true)
            {
                player.TAtk = player.Atk + player.EAtk;
                player.TDef = player.Def + player.EDef;

                choice = -1;
                switch (gameManager.NowScreen)
                {
                    case EScreen.Main:
                        string[] MainOption = { "상태보기", "인벤토리", "상점", "던전 입장", "휴식" };
                        choice = screenManager.DrawBaseScreen("스파르타 마을에 오신 여러분 환영합니다.\n" +
                            "이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n", MainOption, true);
                        switch (choice)
                        {
                            case 1:
                                gameManager.NowScreen = EScreen.Status;
                                break;
                            case 2:
                                gameManager.NowScreen = EScreen.Inventory;
                                break;
                            case 3:
                                gameManager.NowScreen = EScreen.Shop;
                                break;
                            case 4:
                                gameManager.NowScreen = EScreen.DStage;
                                break;
                            case 5:
                                gameManager.NowScreen = EScreen.Rest;
                                break;
                        }
                        break;
                    case EScreen.Status:
                        choice = screenManager.DrawBaseScreen("#상태 보기\n캐릭터의 정보가 표시됩니다.\n", null, false, StatusScreen.Instance);
                        if (choice == 0)
                        {
                            gameManager.NowScreen = EScreen.Main;
                        }
                        break;
                    case EScreen.Inventory:
                        string[] InvenOption = { "장착 관리" };
                        choice = screenManager.DrawBaseScreen("#인벤토리\n보유 중인 아이템을 관리할 수 있습니다.", InvenOption, false, InvemtoryScreen.Instance);
                        switch (choice)
                        {
                            case 0:
                                gameManager.NowScreen = EScreen.Main;
                                break;
                            case 1:
                                gameManager.NowScreen = EScreen.Equip;
                                break;
                        }
                        break;
                    case EScreen.Equip:
                        choice = screenManager.DrawBaseScreen("#인벤토리\n보유 중인 아이템을 관리할 수 있습니다.", null, false, InvemtoryScreen.Instance);
                        if (choice == 0)
                        {
                            gameManager.NowScreen = EScreen.Inventory;
                        }
                        else if (choice > 0 || choice <= player.EquipmentItems.Count)
                        {
                            player.EquipItem(choice);
                        }
                        break;
                    case EScreen.Shop:
                        string[] ShopOption = { "아이템 구매", "아이템 판매" };
                        choice = screenManager.DrawBaseScreen("#상점\n필요한 아이템을 얻을 수 있는 상점입니다.", ShopOption, false, ShopScreen.Instance);
                        switch (choice)
                        {
                            case 0:
                                gameManager.NowScreen = EScreen.Main;
                                break;
                            case 1:
                                gameManager.NowScreen = EScreen.Buy;
                                break;
                            case 2:
                                gameManager.NowScreen = EScreen.Sell;
                                break;
                            default:
                                break;
                        }
                        break;
                    case EScreen.Buy:
                        choice = screenManager.DrawBaseScreen("#상점\n필요한 아이템을 얻을 수 있는 상점입니다.", null, false, ShopScreen.Instance);
                        if (choice == 0)
                        {
                            gameManager.NowScreen = EScreen.Shop;
                        }
                        else if (choice > 0 || choice <= gameManager.ShopItem.Count)
                        {
                            string ItemName = gameManager.ShopItem.Keys.Cast<string>().ElementAt(choice - 1);
                            Item ItemV = gameManager.ShopItem.Values.Cast<Item>().ElementAt(choice - 1);
                            if (player.EquipmentItems.Contains(ItemName))
                            {
                                Console.WriteLine("이미 구매한 아이템입니다.");
                                Console.ReadKey();
                            }
                            else
                            {
                                if (ItemV.Price <= player.Gold)
                                {
                                    player.Gold -= ItemV.Price;
                                    player.AddItem(ItemName, ItemV);
                                    Console.WriteLine("구매를 완료했습니다.");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                    Console.ReadKey();
                                }
                            }
                        }
                        break;
                    case EScreen.Sell:
                        choice = screenManager.DrawBaseScreen("#상점\n필요한 아이템을 얻을 수 있는 상점입니다.", null, false, InvemtoryScreen.Instance);
                        if (choice == 0)
                        {
                            gameManager.NowScreen = EScreen.Shop;
                        }
                        else if (choice > 0 || choice <= player.EquipmentItems.Count)
                        {
                            player.SellItem(choice);
                        }
                        break;
                    case EScreen.DStage:
                        string[] StageOption = { "쉬운 던전  |  방어력 5 이상 권장" , "일반 던전  |  방어력 11 이상 권장" , "어려운 던전  |  방어력 17 이상 권장" };
                        choice = screenManager.DrawBaseScreen("#던전입장\n이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.", StageOption, false);
                        switch (choice)
                        {
                            case 0:
                                gameManager.NowScreen = EScreen.Main;
                                break;
                            case 1:
                            case 2:
                            case 3:
                                gameManager.CalClear(choice);
                                gameManager.NowScreen = EScreen.DClear;
                                break;
                        }
                        break;
                    case EScreen.DClear:
                        choice = screenManager.DrawBaseScreen("#던전", null, false, DungeonScreen.Instance);
                        if (choice == 0)
                        {
                            gameManager.NowScreen = EScreen.Main;
                        }
                        break;
                    case EScreen.Rest:
                        string[] RestOption = { "휴식하기" };
                        choice = screenManager.DrawBaseScreen("#휴식하기\n500 G를 내면 체력을 회복할 수 있습니다.", RestOption, false);
                        switch (choice)
                        {
                            case 0:
                                gameManager.NowScreen = EScreen.Main;
                                break;
                            case 1:
                                if (player.Gold >= 500)
                                {
                                    player.Gold -= 500;

                                    player.Hp = 100;
                                    Console.WriteLine("휴식을 완료했습니다.");
                                    Console.ReadKey();
                                    gameManager.NowScreen = EScreen.Main;
                                }
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                    Console.ReadKey();
                                }
                                break;
                        }
                        break;
                }
            }
        }

        static void AddShop()
        {
            GameManager gameManager = GameManager.Instance;
            //수련자 갑옷
            {
                Item ShopItem = new Item();
                ShopItem.SettingArmor("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
                gameManager.AddShopItem("수련자 갑옷", ShopItem);
            }
            //무쇠 갑옷
            {
                Item ShopItem = new Item();
                ShopItem.SettingArmor("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
                gameManager.AddShopItem("무쇠갑옷", ShopItem);
            }
            //스파르타의 갑옷
            {
                Item ShopItem = new Item();
                ShopItem.SettingArmor("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
                gameManager.AddShopItem("스파르타의 갑옷", ShopItem);
            }

            //낡은 검
            {
                Item ShopItem = new Item();
                ShopItem.SettingWeapon("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
                gameManager.AddShopItem("낡은 검", ShopItem);
            }
            //청동 도끼
            {
                Item ShopItem = new Item();
                ShopItem.SettingWeapon("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
                gameManager.AddShopItem("청동 도끼", ShopItem);
            }
            //스파르타의 창
            {
                Item ShopItem = new Item();
                ShopItem.SettingWeapon("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2100);
                gameManager.AddShopItem("스파르타의 창", ShopItem);
            }
        }

        static void NameSetting()
        {
            bool IsConfirm = false;
            do
            {
                Console.Clear();

                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.");
                Console.Write("\n@이름 : ");
                Player.Instance.Name = Console.ReadLine();

                do
                {
                    Console.WriteLine($"입력하신 이름은 {Player.Instance.Name} 입니다.\n");
                    Console.WriteLine("1. 저장\n2. 취소\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int result))
                    {
                        if (int.Parse(input) < 1 || int.Parse(input) > 2)
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.ReadKey();

                            Console.Clear();
                            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.");
                            Console.Write("\n@이름 : ");
                            Console.WriteLine(Player.Instance.Name);
                            continue;

                        }
                        else // 제대로 입력
                        {
                            if (1 == int.Parse(input))
                            {
                                IsConfirm = false;
                                break;
                            }
                            else if (2 == int.Parse(input))
                            {
                                IsConfirm = true;
                                break;
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();

                        Console.Clear();
                        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.");
                        Console.Write("\n@이름 : ");
                        Console.WriteLine(Player.Instance.Name);
                        continue;
                    }
                }
                while (true);
            }
            while (IsConfirm);
        }

        static void JobSetting()
        {
            Console.Clear();

            string[] jobList = { "전사", "도적" };
            int choice = ScreenManager.Instance.DrawBaseScreen("스파르타 던전에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.\n", jobList, true);

            Player.Instance.Job = jobList[choice - 1];
        }
    }
}
