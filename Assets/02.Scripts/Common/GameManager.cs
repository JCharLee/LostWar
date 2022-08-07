using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSpace;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] itemSlots;
    [SerializeField] private Transform equiped_shortWeapon;
    [SerializeField] private Transform equiped_longWeapon;
    [SerializeField] private Transform equiped_shoes;
    [SerializeField] private Transform equiped_top;
    [SerializeField] private Transform equiped_bottoms;
    private GameObject shortWeapon_C;
    private GameObject longWeapon_C;
    private GameObject shoes_C;
    private GameObject top_C;
    private GameObject bottoms_C;
    private List<GameObject> list_inventory = new List<GameObject>();

    private MoveBehaviour moveBehaviour;
    public static GameManager instance;

    public GameDataObject gameDataObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        itemSlots = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(2).GetComponentsInChildren<Transform>();
        equiped_top = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(1).transform.GetChild(1).GetComponent<Transform>();
        equiped_bottoms = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(1).transform.GetChild(2).GetComponent<Transform>();
        equiped_shoes = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(1).transform.GetChild(3).GetComponent<Transform>();
        equiped_shortWeapon = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(1).transform.GetChild(4).GetComponent<Transform>();
        equiped_longWeapon = GameObject.Find("UI").transform.GetChild(5).transform.GetChild(1).transform.GetChild(5).GetComponent<Transform>();

        moveBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveBehaviour>();
    }

    private void Start()
    {
        shortWeapon_C = null;
        longWeapon_C = null;
        shoes_C = null;
        top_C = null;
        bottoms_C = null;

        gameDataObject.Hp = gameDataObject.MaxHp;

        Reset();
    }

    private void Reset()
    {
        gameDataObject.shortWeapon.Clear();
        gameDataObject.longWeapon.Clear();
        gameDataObject.shoes.Clear();
        gameDataObject.top.Clear();
        gameDataObject.bottoms.Clear();
        gameDataObject.hpPotion.Clear();
        gameDataObject.spPotion.Clear();

        gameDataObject.shortWeapon_C = null;
        gameDataObject.longWeapon_C = null;
        gameDataObject.shoes_C = null;
        gameDataObject.top_C = null;
        gameDataObject.bottoms_C = null;

        gameDataObject.Level = 1;
        gameDataObject.Exp = 0;
        gameDataObject.Exp_require = 100;

        gameDataObject.Str = 5;
        gameDataObject.Agi = 5;
        gameDataObject.Con = 5;
        gameDataObject.Vit = 5;
        gameDataObject.Status_own = 7;
        gameDataObject.MaxHp = 1000;
        gameDataObject.Hp = gameDataObject.MaxHp;
        gameDataObject.MaxSp = 100;
        gameDataObject.Sp = 100;
        gameDataObject.Dam = 5;
        gameDataObject.Def = 5;
        gameDataObject.Weight = 0;
    }

    public void InventoryOn()
    {
        AddInventory(gameDataObject.shortWeapon);
        AddInventory(gameDataObject.longWeapon);
        AddInventory(gameDataObject.shoes);
        AddInventory(gameDataObject.top);
        AddInventory(gameDataObject.bottoms);
        AddInventory(gameDataObject.hpPotion);
        AddInventory(gameDataObject.spPotion);
        shortWeapon_C = AddEquip(equiped_shortWeapon, gameDataObject.shortWeapon_C);
        longWeapon_C = AddEquip(equiped_longWeapon, gameDataObject.longWeapon_C);
        shoes_C = AddEquip(equiped_shoes, gameDataObject.shoes_C);
        top_C = AddEquip(equiped_top, gameDataObject.top_C);
        bottoms_C = AddEquip(equiped_bottoms, gameDataObject.bottoms_C);
    }

    public void EmptyItemObject()
    {
        foreach (GameObject obj in list_inventory)
            Destroy(obj);
        list_inventory.Clear();
        Destroy(shortWeapon_C);
        Destroy(longWeapon_C);
        Destroy(top_C);
        Destroy(bottoms_C);
        Destroy(shoes_C);
    }

    private void AddInventory(List<Item> items)
    {
        int idx = 1;

        for (int i = 1; i < itemSlots.Length + 1; i++)
        {
            if (itemSlots[i].childCount != 0)
                idx++;
            else
                break;
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), itemSlots[idx]);
            obj.GetComponent<SlotItemInfo>().item = item;
            if (item.itemType == ItemType.potion)
            {
                Potion potion = item as Potion;
                obj.GetComponentInChildren<Text>().enabled = true;
                obj.GetComponentInChildren<Text>().text = potion.count.ToString();
            }
            obj.GetComponent<Image>().sprite = item.img;
            list_inventory.Add(obj);
        }
    }

    private GameObject AddEquip(Transform parent, Item item_c)
    {
        if (item_c == null || item_c.name == null || item_c.name == "") return null;
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), parent);
        obj.GetComponent<SlotItemInfo>().item = item_c;
        obj.GetComponent<Image>().sprite = item_c.img;
        obj.GetComponent<SlotItemInfo>().isEquip = true;
        return obj;
    }

    private void Equip(ItemType type)
    {
        Weapon weapon;
        Clothes clothes;

        switch (type)
        {
            case ItemType.shortWeapon:
                if (shortWeapon_C != null)
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shortWeapon);
                gameDataObject.shortWeapon_C = SlotItemInfo.instance.item as Weapon;
                shortWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                weapon = SlotItemInfo.instance.item as Weapon;
                gameDataObject.Dam += weapon.damage;
                gameDataObject.shortWeapon.Remove(SlotItemInfo.instance.item);
                moveBehaviour.ChangeItemObject(SlotItemInfo.instance.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.short_dist;
                break;
            case ItemType.longWeapon:
                if (longWeapon_C != null)
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_longWeapon);
                gameDataObject.longWeapon_C = SlotItemInfo.instance.item as Weapon;
                longWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                weapon = SlotItemInfo.instance.item as Weapon;
                gameDataObject.Dam += weapon.damage;
                gameDataObject.longWeapon.Remove(SlotItemInfo.instance.item);
                moveBehaviour.ChangeItemObject(SlotItemInfo.instance.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.long_dist;
                break;
            case ItemType.shoes:
                if (shoes_C != null)
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shoes);
                gameDataObject.shoes_C = SlotItemInfo.instance.item as Clothes;
                shoes_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.shoes.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.top:
                if (top_C != null)
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                SlotItemInfo.instance.transform.SetParent(equiped_top);
                gameDataObject.top_C = SlotItemInfo.instance.item as Clothes;
                top_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.top.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.bottoms:
                if (bottoms_C != null)
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                SlotItemInfo.instance.transform.SetParent(equiped_bottoms);
                gameDataObject.bottoms_C = SlotItemInfo.instance.item as Clothes;
                bottoms_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.bottoms.Remove(SlotItemInfo.instance.item);
                break;
        }
    }

    private void OffEquip(List<Item> list_item, Item item_c, GameObject object_c)
    {
        int idx = 1;

        for (int i = 1; i < itemSlots.Length + 1; i++)
        {
            if (itemSlots[i].childCount != 0)
                idx++;
            else
                break;
        }

        list_item.Add(item_c);
        object_c.transform.SetParent(itemSlots[idx]);
        SlotItemInfo temp = object_c.GetComponent<SlotItemInfo>();
        temp.isEquip = false;
        gameDataObject.Str -= temp.item.str;
        gameDataObject.Agi -= temp.item.agi;
        gameDataObject.Con -= temp.item.con;
        gameDataObject.Vit -= temp.item.vit;
        SetState(false, temp.item.str, temp.item.agi, temp.item.con, temp.item.vit);

        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon = temp.item as Weapon;
            gameDataObject.Dam -= weapon.damage;
        }
        else
        {
            Clothes clothes = temp.item as Clothes;
            gameDataObject.Def -= clothes.def;
        }
    }

    public void SetState(bool set, int str, int agi, int con, int vit)
    {
        if (set)
        {
            gameDataObject.MaxHp += (((float)str * 10f) + ((float)con * 50f));
            gameDataObject.MaxSp += (((float)con * 10f) + ((float)vit * 20f));
            gameDataObject.Dam += (((float)str * 5f) + ((float)agi * 2f));
            gameDataObject.Def += (((float)str * 2f) + ((float)con * 5f));
        }
        else
        {
            gameDataObject.MaxHp -= (((float)str * 10f) + ((float)con * 50f));
            gameDataObject.MaxSp -= (((float)con * 10f) + ((float)vit * 20f));
            gameDataObject.Dam -= (((float)str * 5f) + ((float)agi * 2f));
            gameDataObject.Def -= (((float)str * 2f) + ((float)con * 5f));
        }
    }

    public void OnDoubleClickEquip()
    {
        if (SlotItemInfo.instance != null)
        {
            if (!SlotItemInfo.instance.isEquip)
                Equip(SlotItemInfo.instance.item.itemType);
        }
    }

    public void OnDoubleClickEquipOff()
    {
        if (SlotItemInfo.instance.isEquip)
        {
            switch (SlotItemInfo.instance.item.itemType)
            {
                case ItemType.shortWeapon:
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                    gameDataObject.shortWeapon_C = null;
                    shortWeapon_C = null;
                    break;
                case ItemType.longWeapon:
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                    gameDataObject.longWeapon_C = null;
                    longWeapon_C = null;
                    break;
                case ItemType.top:
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                    gameDataObject.top_C = null;
                    top_C = null;
                    break;
                case ItemType.bottoms:
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                    gameDataObject.bottoms_C = null;
                    bottoms_C = null;
                    break;
                case ItemType.shoes:
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                    gameDataObject.shoes_C = null;
                    shoes_C = null;
                    break;
            }

            if (!list_inventory.Contains(SlotItemInfo.instance.gameObject))
                list_inventory.Add(SlotItemInfo.instance.gameObject);
        }
    }
}