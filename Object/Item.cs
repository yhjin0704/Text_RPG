using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum EEquipPart
{
    Weapon,
    Armor,
}

namespace Text_RPG
{
    internal class Item
    {
        public Item() 
        {
            Isequip = false;
        }

        public string Name;
        public float Atk;
        public float Def;
        public string Explain;
        public bool Isequip;
        public EEquipPart EEP;
        public int Price; 

        public void SettingArmor(string _Name, float _Def, string _Explain, int _Price)
        {
            Name = _Name;
            Atk = 0;
            Def = _Def;
            Explain = _Explain;
            EEP = EEquipPart.Armor;
            Price = _Price;
        }

        public void SettingWeapon(string _Name, float _Atk, string _Explain, int _Price)
        {
            Name = _Name;
            Atk = _Atk;
            Def = 0;
            Explain = _Explain;
            EEP = EEquipPart.Weapon;
            Price = _Price;
        }
    }
}
