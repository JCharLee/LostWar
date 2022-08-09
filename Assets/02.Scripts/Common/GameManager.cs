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

        itemSlots = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(2).GetComponentsInChildren<Transform>();
        equiped_top = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(1).transform.GetChild(1).GetComponent<Transform>();
        equiped_bottoms = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(1).transform.GetChild(2).GetComponent<Transform>();
        equiped_shoes = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(1).transform.GetChild(3).GetComponent<Transform>();
        equiped_shortWeapon = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(1).transform.GetChild(4).GetComponent<Transform>();
        equiped_longWeapon = GameObject.Find("UI").transform.GetChild(6).transform.GetChild(1).transform.GetChild(5).GetComponent<Transform>();

        moveBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveBehaviour>();
    }

    private void Start()
    {
        shortWeapon_C = null;
        longWeapon_C = null;
        shoes_C = null;
        top_C = null;
        bottoms_C = null;

        DataManager.instance.gameData.hp = DataManager.instance.gameData.maxHp;
        DataManager.instance.gameData.sp = DataManager.instance.gameData.maxSp;

        if (PlayerInteraction.instance.currentMapName == "Level1")
        {
            Reset();
        }
    }

    private void Reset()
    {
        DataManager.instance.gameData.shortWeapon.Clear();
        DataManager.instance.gameData.longWeapon.Clear();
        DataManager.instance.gameData.shoes.Clear();
        DataManager.instance.gameData.top.Clear();
        DataManager.instance.gameData.bottoms.Clear();
        DataManager.instance.gameData.hpPotion.Clear();
        DataManager.instance.gameData.spPotion.Clear();

        DataManager.instance.gameData.shortWeaponC = null;
        DataManager.instance.gameData.longWeaponC = null;
        DataManager.instance.gameData.shoesC = null;
        DataManager.instance.gameData.topC = null;
        DataManager.instance.gameData.bottomsC = null;

        DataManager.instance.gameData.level = 1;
        DataManager.instance.gameData.exp = 0;
        DataManager.instance.gameData.expRequire = 100;

        DataManager.instance.gameData.str = 5;
        DataManager.instance.gameData.agi = 5;
        DataManager.instance.gameData.con = 5;
        DataManager.instance.gameData.vit = 5;
        DataManager.instance.gameData.maxHp = 1000;
        DataManager.instance.gameData.hp = DataManager.instance.gameData.maxHp;
        DataManager.instance.gameData.maxSp = 100;
        DataManager.instance.gameData.sp = DataManager.instance.gameData.maxSp;
        DataManager.instance.gameData.dam = 5;
        DataManager.instance.gameData.def = 5;
    }

    public void InventoryOn()
    {
        AddInventory(DataManager.instance.gameData.shortWeapon);
        AddInventory(DataManager.instance.gameData.longWeapon);
        AddInventory(DataManager.instance.gameData.shoes);
        AddInventory(DataManager.instance.gameData.top);
        AddInventory(DataManager.instance.gameData.bottoms);
        AddInventory(DataManager.instance.gameData.hpPotion);
        AddInventory(DataManager.instance.gameData.spPotion);
        shortWeapon_C = AddEquip(equiped_shortWeapon, DataManager.instance.gameData.shortWeaponC);
        longWeapon_C = AddEquip(equiped_longWeapon, DataManager.instance.gameData.longWeaponC);
        shoes_C = AddEquip(equiped_shoes, DataManager.instance.gameData.shoesC);
        top_C = AddEquip(equiped_top, DataManager.instance.gameData.topC);
        bottoms_C = AddEquip(equiped_bottoms, DataManager.instance.gameData.bottomsC);
    }

    public void EmptyItemObject()
    {
        foreach (GameObject obj in list_inventory)
            Destroy(obj);
        list_inventory.Clear();
        DataManager.instance.gameData.slotItemInfos.Clear();
        Destroy(shortWeapon_C);
        Destroy(longWeapon_C);
        Destroy(top_C);
        Destroy(bottoms_C);
        Destroy(shoes_C);
    }

    #region [인벤토리 데이터 반영]
    private void AddInventory<T>(List<T> items) where T : Item
    {
        for (int i = 0; i < items.Count; i++)
        {
            int idx = 1;
            for (int j = 1; j < itemSlots.Length; j++)
            {
                if (itemSlots[j].childCount != 0)
                    idx++;
                else
                    break;
            }

            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), itemSlots[idx]);
            obj.GetComponent<SlotItemInfo>().item = items[i];
            if (items[i].itemType == ItemType.potion)
            {
                Potion potion = items[i] as Potion;
                obj.GetComponentInChildren<Text>().enabled = true;
                obj.GetComponentInChildren<Text>().text = potion.count.ToString();
            }
            obj.GetComponent<Image>().sprite = items[i].img;
            list_inventory.Add(obj);
            DataManager.instance.gameData.slotItemInfos.Add(obj.GetComponent<SlotItemInfo>());
        }
    }

    private GameObject AddEquip(Transform parent, Item item_c)
    {
        if (item_c == null || item_c.name == null || item_c.name == "") return null;

        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon = item_c as Weapon;
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), parent);
            obj.GetComponent<SlotItemInfo>().item = weapon;
            obj.GetComponent<Image>().sprite = weapon.img;
            obj.GetComponent<SlotItemInfo>().isEquip = true;
            DataManager.instance.gameData.slotItemInfos.Add(obj.GetComponent<SlotItemInfo>());
            return obj;
        }
        else
        {
            Clothes clothes = item_c as Clothes;
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), parent);
            obj.GetComponent<SlotItemInfo>().item = clothes;
            obj.GetComponent<Image>().sprite = clothes.img;
            obj.GetComponent<SlotItemInfo>().isEquip = true;
            DataManager.instance.gameData.slotItemInfos.Add(obj.GetComponent<SlotItemInfo>());
            return obj;
        }
    }
    #endregion

    #region [장비 착용 및 해제]
    private void Equip(ItemType type)
    {
        Weapon weapon;
        Clothes clothes;

        switch (type)
        {
            case ItemType.shortWeapon:
                if (shortWeapon_C != null)
                    OffEquip(DataManager.instance.gameData.shortWeapon, DataManager.instance.gameData.shortWeaponC, shortWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shortWeapon);
                DataManager.instance.gameData.shortWeaponC = SlotItemInfo.instance.item as Weapon;
                shortWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                DataManager.instance.gameData.str += SlotItemInfo.instance.item.str;
                DataManager.instance.gameData.agi += SlotItemInfo.instance.item.agi;
                DataManager.instance.gameData.con += SlotItemInfo.instance.item.con;
                DataManager.instance.gameData.vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                weapon = SlotItemInfo.instance.item as Weapon;
                DataManager.instance.gameData.dam += weapon.damage;
                DataManager.instance.gameData.shortWeapon.Remove(SlotItemInfo.instance.item);
                moveBehaviour.ChangeItemObject(SlotItemInfo.instance.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.short_dist;
                break;
            case ItemType.longWeapon:
                if (longWeapon_C != null)
                    OffEquip(DataManager.instance.gameData.longWeapon, DataManager.instance.gameData.longWeaponC, longWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_longWeapon);
                DataManager.instance.gameData.longWeaponC = SlotItemInfo.instance.item as Weapon;
                longWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                DataManager.instance.gameData.str += SlotItemInfo.instance.item.str;
                DataManager.instance.gameData.agi += SlotItemInfo.instance.item.agi;
                DataManager.instance.gameData.con += SlotItemInfo.instance.item.con;
                DataManager.instance.gameData.vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                weapon = SlotItemInfo.instance.item as Weapon;
                DataManager.instance.gameData.dam += weapon.damage;
                DataManager.instance.gameData.longWeapon.Remove(SlotItemInfo.instance.item);
                moveBehaviour.ChangeItemObject(SlotItemInfo.instance.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.long_dist;
                break;
            case ItemType.shoes:
                if (shoes_C != null)
                    OffEquip(DataManager.instance.gameData.shoes, DataManager.instance.gameData.shoesC, shoes_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shoes);
                DataManager.instance.gameData.shoesC = SlotItemInfo.instance.item as Clothes;
                shoes_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                DataManager.instance.gameData.str += SlotItemInfo.instance.item.str;
                DataManager.instance.gameData.agi += SlotItemInfo.instance.item.agi;
                DataManager.instance.gameData.con += SlotItemInfo.instance.item.con;
                DataManager.instance.gameData.vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                DataManager.instance.gameData.def += clothes.def;
                DataManager.instance.gameData.shoes.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.top:
                if (top_C != null)
                    OffEquip(DataManager.instance.gameData.top, DataManager.instance.gameData.topC, top_C);
                SlotItemInfo.instance.transform.SetParent(equiped_top);
                DataManager.instance.gameData.topC = SlotItemInfo.instance.item as Clothes;
                top_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                DataManager.instance.gameData.str += SlotItemInfo.instance.item.str;
                DataManager.instance.gameData.agi += SlotItemInfo.instance.item.agi;
                DataManager.instance.gameData.con += SlotItemInfo.instance.item.con;
                DataManager.instance.gameData.vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                DataManager.instance.gameData.def += clothes.def;
                DataManager.instance.gameData.top.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.bottoms:
                if (bottoms_C != null)
                    OffEquip(DataManager.instance.gameData.bottoms, DataManager.instance.gameData.bottomsC, bottoms_C);
                SlotItemInfo.instance.transform.SetParent(equiped_bottoms);
                DataManager.instance.gameData.bottomsC = SlotItemInfo.instance.item as Clothes;
                bottoms_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                DataManager.instance.gameData.str += SlotItemInfo.instance.item.str;
                DataManager.instance.gameData.agi += SlotItemInfo.instance.item.agi;
                DataManager.instance.gameData.con += SlotItemInfo.instance.item.con;
                DataManager.instance.gameData.vit += SlotItemInfo.instance.item.vit;
                SetState(true, SlotItemInfo.instance.item.str, SlotItemInfo.instance.item.agi, SlotItemInfo.instance.item.con, SlotItemInfo.instance.item.vit);
                clothes = SlotItemInfo.instance.item as Clothes;
                DataManager.instance.gameData.def += clothes.def;
                DataManager.instance.gameData.bottoms.Remove(SlotItemInfo.instance.item);
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
        DataManager.instance.gameData.str -= temp.item.str;
        DataManager.instance.gameData.agi -= temp.item.agi;
        DataManager.instance.gameData.con -= temp.item.con;
        DataManager.instance.gameData.vit -= temp.item.vit;
        SetState(false, temp.item.str, temp.item.agi, temp.item.con, temp.item.vit);

        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon = temp.item as Weapon;
            DataManager.instance.gameData.dam -= weapon.damage;
        }
        else
        {
            Clothes clothes = temp.item as Clothes;
            DataManager.instance.gameData.def -= clothes.def;
        }
    }

    public void SetState(bool set, int str, int agi, int con, int vit)
    {
        if (set)
        {
            DataManager.instance.gameData.maxHp += (((float)str * 10f) + ((float)con * 50f));
            DataManager.instance.gameData.maxSp += (((float)con * 10f) + ((float)vit * 20f));
            DataManager.instance.gameData.dam += (((float)str * 5f) + ((float)agi * 2f));
            DataManager.instance.gameData.def += (((float)str * 2f) + ((float)con * 5f));
        }
        else
        {
            DataManager.instance.gameData.maxHp -= (((float)str * 10f) + ((float)con * 50f));
            DataManager.instance.gameData.maxSp -= (((float)con * 10f) + ((float)vit * 20f));
            DataManager.instance.gameData.dam -= (((float)str * 5f) + ((float)agi * 2f));
            DataManager.instance.gameData.def -= (((float)str * 2f) + ((float)con * 5f));
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
                    OffEquip(DataManager.instance.gameData.shortWeapon, DataManager.instance.gameData.shortWeaponC, shortWeapon_C);
                    DataManager.instance.gameData.shortWeaponC = null;
                    shortWeapon_C = null;
                    break;
                case ItemType.longWeapon:
                    OffEquip(DataManager.instance.gameData.longWeapon, DataManager.instance.gameData.longWeaponC, longWeapon_C);
                    DataManager.instance.gameData.longWeaponC = null;
                    longWeapon_C = null;
                    break;
                case ItemType.top:
                    OffEquip(DataManager.instance.gameData.top, DataManager.instance.gameData.topC, top_C);
                    DataManager.instance.gameData.topC = null;
                    top_C = null;
                    break;
                case ItemType.bottoms:
                    OffEquip(DataManager.instance.gameData.bottoms, DataManager.instance.gameData.bottomsC, bottoms_C);
                    DataManager.instance.gameData.bottomsC = null;
                    bottoms_C = null;
                    break;
                case ItemType.shoes:
                    OffEquip(DataManager.instance.gameData.shoes, DataManager.instance.gameData.shoesC, shoes_C);
                    DataManager.instance.gameData.shoesC = null;
                    shoes_C = null;
                    break;
            }
            if (!list_inventory.Contains(SlotItemInfo.instance.gameObject))
            {
                list_inventory.Add(SlotItemInfo.instance.gameObject);
                DataManager.instance.gameData.slotItemInfos.Add(SlotItemInfo.instance.gameObject.GetComponent<SlotItemInfo>());
            }
        }
    }
    #endregion

    #region [포션 업데이트]
    public void UpdatePotion(Potion potion)
    {
        if (potion.count <= 0)
        {
            switch (potion.potionType)
            {
                case PotionType.HP:
                    DataManager.instance.gameData.hpPotion.Remove(potion);
                    break;
                case PotionType.SP:
                    DataManager.instance.gameData.spPotion.Remove(potion);
                    break;
            }
        }
    }

    public void UsePotion(Potion potion)
    {
        switch (potion.potionType)
        {
            case PotionType.HP:
                if (DataManager.instance.gameData.hp >= DataManager.instance.gameData.maxHp)
                {
                    StartCoroutine(UIManager.instance.NoticeText(false, "이미 체력이 가득 차 있습니다."));
                    return;
                }
                break;
            case PotionType.SP:
                if (DataManager.instance.gameData.sp >= DataManager.instance.gameData.maxSp)
                {
                    StartCoroutine(UIManager.instance.NoticeText(false, "이미 기력이 가득 차 있습니다."));
                    return;
                }
                break;
        }
        potion.Use();
        UpdatePotion(potion);
    }
    #endregion
}