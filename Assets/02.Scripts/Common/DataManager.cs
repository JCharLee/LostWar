using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ItemSpace;

[System.Serializable]
public class GameData
{
    public int level = 1;
    public float expRequire = 100f;
    [SerializeField] private float Exp = 0;
    public float exp
    {
        get
        {
            return Exp;
        }
        set
        {
            Exp = value;
            if (Exp >= expRequire)
            {
                Exp -= expRequire;
                level++;
                expRequire *= 1.2f;
                UIManager.instance.UpdateLevel(level);
            }
        }
    }
    [SerializeField] private float MaxHp = 1000f;
    [SerializeField] private float Hp;
    [SerializeField] private float MaxSp = 100f;
    [SerializeField] private float Sp;
    public float maxHp
    {
        get
        {
            return MaxHp;
        }
        set
        {
            MaxHp = value;
            if (hp > MaxHp)
            {
                hp -= (hp - MaxHp);
                UIManager.instance.DisplayHpBar();
            }
        }
    }
    public float hp
    {
        get
        {
            return Hp;
        }
        set
        {
            Hp = value;
            Hp = Mathf.Clamp(Hp, 0f, maxHp);
        }
    }
    public float maxSp
    {
        get
        {
            return MaxSp;
        }
        set
        {
            MaxSp = value;
            if (sp > MaxSp)
            {
                sp -= (sp - MaxSp);
                UIManager.instance.DisplaySpBar();
            }
        }
    }
    public float sp
    {
        get
        {
            return Sp;
        }
        set
        {
            Sp = value;
            Sp = Mathf.Clamp(Sp, 0f, maxSp);
        }
    }
    public int str = 5;
    public int agi = 5;
    public int con = 5;
    public int vit = 5;
    public float dam = 5;
    public float def = 5;

    public List<Item> shortWeapon;
    public List<Item> longWeapon;
    public List<Item> shoes;
    public List<Item> top;
    public List<Item> bottoms;
    public List<Potion> hpPotion;
    public List<Potion> spPotion;

    public Weapon shortWeaponC;
    public Weapon longWeaponC;
    public Clothes shoesC;
    public Clothes topC;
    public Clothes bottomsC;

    public int questId;
    public int questActionIdx;
    public QuestData questData;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public GameData gameData = new GameData();

    private string path;
    string fileName = "/save";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + fileName;
    }

    public void SaveData()
    {
        Debug.Log("save");
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, data);
        Debug.Log(data);
    }

    public void LoadData()
    {
        Debug.Log("load");
        string data = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(data);
        Debug.Log(data);
    }

    public void ClearData()
    {
        gameData = new GameData();
    }
}
