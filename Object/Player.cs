using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;

namespace Text_RPG
{
    internal class Player
    {
        private static Player instance = new Player();

        private Player()
        {
            Level = 1;
            Atk = 10;
            EAtk = 0;
            TAtk = Atk + EAtk;
            Def = 5;
            EDef = 0;
            TDef = Def + EDef;
            Hp = 100;
            Gold = 1500;
            DClearStack = 0;
            EquipmentItems = new OrderedDictionary();
        }

        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        public string Name { get; set; }
        public string Job { get; set; }
        public uint Level { get; set; }
        public float TAtk { get; set; }
        public float Atk { get; set; }
        public float EAtk { get; set; }
        public float TDef { get; set; }
        public float Def { get; set; }
        public float EDef { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }
        public int DClearStack { get; set; }
        public OrderedDictionary EquipmentItems { get; set; }

        public void AddItem(string _Name, Item _Item)
        {
            if (!EquipmentItems.Contains(_Name))
            {
                EquipmentItems.Add(_Name, _Item);
            }
        }
        public void SellItem(int _index)
        {
            Item CheckItem = (Item)EquipmentItems[_index - 1];
            if (CheckItem.Isequip == true)
            {
                EAtk -= CheckItem.Atk;
                EDef -= CheckItem.Def;
            }
            double Sellprice = CheckItem.Price * 0.85;
            Sellprice = Math.Floor(Sellprice);
            Player.instance.Gold += (int)Sellprice;
            EquipmentItems.RemoveAt(_index-1);
        }

        public void EquipItem(int _index)
        {
            Item CheckItem = (Item)EquipmentItems[_index-1];
            if (false == CheckItem.Isequip)
            {
                Item? EquipCheckItem = EquipmentItems.Values.Cast<Item>().Where(item => item.EEP == CheckItem.EEP).FirstOrDefault(item => item.Isequip);
                if (null != EquipCheckItem)
                {
                    EAtk -= EquipCheckItem.Atk;
                    EDef -= EquipCheckItem.Def;
                    EquipCheckItem.Isequip = false;
                }

                EAtk += CheckItem.Atk;
                EDef += CheckItem.Def;
                CheckItem.Isequip = true;
            }
            else
            {
                EAtk -= CheckItem.Atk;
                EDef -= CheckItem.Def;
                CheckItem.Isequip = false;
            }
        }
        public void LevelupCheck()
        {
            if (DClearStack == Level)
            {
                DClearStack = 0;
                Level++;
                Atk += 0.5f;
                Def += 1;
            }
        }
    }
}
