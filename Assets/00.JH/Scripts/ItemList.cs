using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSpace
{
    public class ItemList : MonoBehaviour//방어구 아이템 리스트
    {
        //아이템 목록, 무기는 장착중인 것 넣어야한다.
        [SerializeField] private List<string> Shoes;
        [SerializeField] private List<string> Top;
        [SerializeField] private List<string> Bottoms;
        [SerializeField] private List<string> Potions;
        [SerializeField] private Sprite[] itemImage;
        public static ItemList instance;
        private void Awake()
        {
            instance = this;
            Shoes = new List<string>();
            Top = new List<string>();
            Bottoms = new List<string>();
            Potions = new List<string>();
            Init_Shoes();
            Init_Top();
            Init_Bottoms();
            Init_Potions();
        }

        private void Init_Bottoms()// 하의 아이템 리스트
        {
            Bottoms.Add("Pants");
            Bottoms.Add("Jeans");
        }

        private void Init_Top()// 상의 아이템 리스트
        {
            Top.Add("Shirts");
            Top.Add("Coat");
        }

        private void Init_Shoes()// 신발 아이템 리스트
        {
            Shoes.Add("Boots");
            Shoes.Add("Sneakers");
        }

        private void Init_Potions()
        {
            Potions.Add("HP Potion");
            Potions.Add("SP Potion");
        }

        public Item Get(string _name)// 아이템 스탯 무작위 지정
        {
            switch (_name)
            {
                //근거리 무기
                case "Sword":
                    return new Weapon("Sword", Random.Range(2, 5), Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon, itemImage[0]);
                case "Axe":
                    return new Weapon("Axe", Random.Range(2, 5), Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon, itemImage[1]);
                //원거리 무기
                case "Pistol":
                    return new Weapon("Pistol", Random.Range(1, 3), Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon, itemImage[2]);
                case "Revolver":
                    return new Weapon("Revolver", Random.Range(1, 3), Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon, itemImage[3]);
                //신발
                case "Boots":
                    return new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[4]);
                case "Sneakers":
                    return new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[5]);
                //상의
                case "Shirts":
                    return new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.top, itemImage[6]);
                case "Coat":
                    return new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.top, itemImage[7]);
                //하의
                case "Pants":
                    return new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.bottoms, itemImage[8]);
                case "Jeans":
                    return new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.bottoms, itemImage[9]);
                case "HP Potion":
                    return new Potion("HP Potion", ItemType.potion, PotionType.HP, 500f, 5, itemImage[10]);
                case "SP Potion":
                    return new Potion("SP Potion", ItemType.potion, PotionType.SP, 50f, 5, itemImage[11]);
                default:
                    return null;
            }
        }
        public Item[] GetRandom()// 아이템 랜덤 드랍
        {
            Item[] item = new Item[3];// 드랍되는 아이템은 3개

            int randint = Random.Range(0, 5);
            switch (randint)// 첫 번째 아이템 드랍률
            {
                case 0:
                    item[0] = new Weapon("Sword", Random.Range(2, 5), Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon, itemImage[0]);
                    break;
                case 1:
                    item[0] = new Weapon("Axe", Random.Range(2, 5), Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon, itemImage[1]);
                    break;
                case 2:
                    item[0] = new Weapon("Pistol", Random.Range(1, 3), Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon, itemImage[2]);
                    break;
                case 3:
                    item[0] = new Weapon("Revolver", Random.Range(1, 3), Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon, itemImage[3]);
                    break;
                default:
                    item[0] = null;
                    break;
            }
            randint = Random.Range(0, 9);
            switch (randint)// 두 번째 아이템 드랍률
            {
                case 0:
                    item[1] = new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[4]);
                    break;
                case 1:
                    item[1] = new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[5]);
                    break;
                case 2:
                    item[1] = new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top, itemImage[6]);
                    break;
                case 3:
                    item[1] = new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top, itemImage[7]);
                    break;
                case 4:
                    item[1] = new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms, itemImage[8]);
                    break;
                case 5:
                    item[1] = new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms, itemImage[9]);
                    break;
                case 6:
                    item[1] = new Potion("HP Potion", ItemType.potion, PotionType.HP, 500f, Random.Range(1, 3), itemImage[10]);
                    break;
                case 7:
                    item[1] = new Potion("SP Potion", ItemType.potion, PotionType.SP, 50f, Random.Range(1, 3), itemImage[11]);
                    break;
                default:
                    item[1] = null;
                    break;
            }
            randint = Random.Range(0, 9);
            switch (randint)// 세 번째 아이템 드랍률
            {
                case 0:
                    item[2] = new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[4]);
                    break;
                case 1:
                    item[2] = new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes, itemImage[5]);
                    break;
                case 2:
                    item[2] = new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top, itemImage[6]);
                    break;
                case 3:
                    item[2] = new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top, itemImage[7]);
                    break;
                case 4:
                    item[2] = new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms, itemImage[8]);
                    break;
                case 5:
                    item[2] = new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms, itemImage[9]);
                    break;
                case 6:
                    item[2] = new Potion("HP Potion", ItemType.potion, PotionType.HP, 500f, Random.Range(1, 3), itemImage[10]);
                    break;
                case 7:
                    item[2] = new Potion("SP Potion", ItemType.potion, PotionType.SP, 50f, Random.Range(1, 3), itemImage[11]);
                    break;
                default:
                    item[2] = null;
                    break;
            }
            return item;// 아이템 데이터 반환
        }
    }
}