using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;


namespace Text_RPG
{
    internal class GameManager
    {
        private static GameManager instance = new GameManager();

        private GameManager()
        {
            random = new Random();
            ShopItem = new OrderedDictionary();
            DHp = 0;
            DGold = 0;
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        Random random;
        public EScreen NowScreen { get; set; }

        public int StageLevel = 1;
        public bool IsClear = false;
        public int DChoice {  get; set; }
        public int DHp { get; set; }
        public int DGold { get; set; }

        public OrderedDictionary ShopItem { get; set; }
        public void AddShopItem(string _Name, Item _Item)
        {
            if (!ShopItem.Contains(_Name))
            {
                ShopItem.Add(_Name, _Item);
            }
        }

        public void CalClear(int _SageLevelChoice)
        {
            int DSDef = 0;
            DChoice = _SageLevelChoice;
            switch (_SageLevelChoice)
            {
                case 1:
                    DSDef = 5;
                    break;
                case 2:
                    DSDef = 11;
                    break;
                case 3:
                    DSDef = 17;
                    break;
            }

            if (Player.Instance.TDef >= DSDef)
            {
                DHp = Player.Instance.Hp - random.Next(20 - ((int)Player.Instance.TDef - DSDef), 35 - ((int)Player.Instance.TDef - DSDef));
                CalReward(_SageLevelChoice);
                IsClear = true;
            }
            else
            {
                DHp = (int)(Player.Instance.Hp / 2);
                if (5 < random.Next(0, 10))
                {
                    IsClear = false;
                }
                else
                {
                    CalReward(_SageLevelChoice);
                    IsClear = true;
                }
            }

        }

        void CalReward(int _SageLevelChoice)
        {
            int Reward = 0;
            switch (_SageLevelChoice)
            {
                case 1:
                    Reward = 1000;
                    break;
                case 2:
                    Reward = 1700;
                    break;
                case 3:
                    Reward = 2500;
                    break;
            }
            float Bonus = (float)random.Next((int)Player.Instance.TAtk, (int)Player.Instance.TAtk * 2) * 0.01f;

            Reward += (int)(Reward * Bonus);
            DGold = Player.Instance.Gold + Reward;
        }
    }
}
