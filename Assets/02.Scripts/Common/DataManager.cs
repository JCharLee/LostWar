using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ItemSpace;

[System.Serializable]
public class GameData
{
    public int level;
    public float expRequire;
    public float exp;
    public float maxHp;
    public float hp;
    public float maxSp;
    public float sp;

    public int str;
    public int agi;
    public int con;
    public int vit;
    public float dam;
    public float def;

    public List<Item> shortWeapon;
    public List<Item> longWeapon;
    public List<Item> shoes;
    public List<Item> top;
    public List<Item> bottoms;
    public List<Item> hpPotion;
    public List<Item> spPotion;

    public Weapon shortWeaponC;
    public Weapon longWeaponC;
    public Clothes shoesC;
    public Clothes topC;
    public Clothes bottomsC;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField] private GameData gameData;

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
    }

    public void LoadData()
    {
        Debug.Log("load");
        string data = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(data);
    }
}
