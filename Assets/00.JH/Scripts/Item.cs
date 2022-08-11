using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSpace
{
    public enum ItemType { shortWeapon, longWeapon, shoes, top, bottoms, potion }// 장비 부위 분류
    public enum PotionType { HP, SP }
    [System.Serializable]
    public class Item
    {
        public string name;// 아이템 이름
        public int str;// 아이템 힘 스탯
        public int agi;// 아이템 민첩 스탯
        public int con;// 아이템 컨디션 스탯
        public int vit;// 아이템 바이탈 스탯
        public int num;// ?
        public Sprite img;
        
        public float weight;//무게
        public float durability;//내구도
        public ItemType itemType;// 아이템 타입
        public Item()//생성자
        {
            durability = 100f;//내구도 100
        }
    }
    [System.Serializable]
    public class Clothes : Item// 아이템 클래스를 상속받는 방어구 장비
        {
            public float def;//방어도
            public Clothes(string _name, int _str, int _agi, int _con, int _vit, float _def, float _weight,  ItemType _itemType, Sprite image)//이름, 능력치, 텍스쳐, 부위()
            {
                itemType = _itemType;
                name = _name;
                str = _str;
                agi = _agi;
                con = _con;
                vit = _vit;
                def = _def;
                weight = _weight;
                img = image;
            }
        }
    [System.Serializable]
    public class Weapon : Item// 아이템 클래스를 상속받는 무기 장비
    {
        public float damage;//공격력
        public Weapon(string _name, int _str, int _agi, int _con, int _vit, float _damage, float _weight, ItemType _itemType, Sprite image)
        {
            name = _name; str = _str; agi = _agi; con = _con; vit = _vit; damage = _damage; weight = _weight; itemType = _itemType; img = image;
        }
    }

    [System.Serializable]
    public class Potion : Item
    {
        public PotionType potionType;
        public float amount;
        public int count;

        public Potion(string _name, ItemType itype, PotionType ptype, float _amount, int _count, Sprite image)
        {
            name = _name;
            itemType = itype;
            potionType = ptype;
            amount = _amount;
            count = _count;
            img = image;
        }

        public void Use()
        {
            switch (potionType)
            {
                case PotionType.HP:
                    DataManager.instance.gameData.hp += amount;
                    break;
                case PotionType.SP:
                    DataManager.instance.gameData.sp += amount;
                    break;
            }
            count--;
        }
    }
}