using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class GetDropItem : MonoBehaviour
{
    public int itemIdx;
    public UIManager uiManager;
    private QuestManager questManager;
    private PlayerInteraction player;

    private void Awake()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = FindObjectOfType<PlayerInteraction>();
    }

    public void GetItem()
    {
        if (questManager.QuestList[DataManager.instance.gameData.questId].goal.goalType == GoalType.GATHERING)
        {
            if (DataManager.instance.gameData.questId == 10)
            {
                if (uiManager.items[itemIdx].name == "Sword" || uiManager.items[itemIdx].name == "Pistol" || uiManager.items[itemIdx].name == "HP Potion")
                    player.Collect();
            }
        }

        if (uiManager.items[itemIdx].itemType == ItemType.shortWeapon)
        {
            DataManager.instance.gameData.shortWeapon.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.longWeapon)
        {
            DataManager.instance.gameData.longWeapon.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.shoes)
        {
            DataManager.instance.gameData.shoes.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.top)
        {
            DataManager.instance.gameData.top.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.bottoms)
        {
            DataManager.instance.gameData.bottoms.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.potion)
        {
            Potion newPotion = uiManager.items[itemIdx] as Potion;
            switch (newPotion.potionType)
            {
                case PotionType.HP:
                    if (DataManager.instance.gameData.hpPotion.Contains(DataManager.instance.gameData.hpPotion.Find(x => x.name == "HP Potion")))
                    {
                        Potion curPotion = DataManager.instance.gameData.hpPotion.Find(x => x.name == "HP Potion") as Potion;
                        curPotion.count += newPotion.count;
                    }
                    else
                        DataManager.instance.gameData.hpPotion.Add(uiManager.items[itemIdx]);
                    break;
                case PotionType.SP:
                    if (DataManager.instance.gameData.spPotion.Contains(DataManager.instance.gameData.spPotion.Find(x => x.name == "SP Potion")))
                    {
                        Potion curPotion = DataManager.instance.gameData.spPotion.Find(x => x.name == "SP Potion") as Potion;
                        curPotion.count += newPotion.count;
                    }
                    else
                        DataManager.instance.gameData.spPotion.Add(uiManager.items[itemIdx]);
                    break;
            }
            uiManager.items[itemIdx] = null;
        }
        Destroy(gameObject);
    }
}