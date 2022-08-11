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
        if (DataManager.instance.gameData.shortWeapon.Count +
            DataManager.instance.gameData.longWeapon.Count +
            DataManager.instance.gameData.shoes.Count +
            DataManager.instance.gameData.top.Count +
            DataManager.instance.gameData.bottoms.Count +
            DataManager.instance.gameData.hpPotion.Count +
            DataManager.instance.gameData.spPotion.Count == GameManager.instance.itemSlots.Length - 1)
        {
            if (uiManager.items[itemIdx].itemType != ItemType.potion)
            {
                if (uiManager.alert) return;
                StartCoroutine(uiManager.NoticeText(false, "가방이 가득 찼습니다."));
                return;
            }
            else
            {
                Potion potion = uiManager.items[itemIdx] as Potion;
                switch (potion.potionType)
                {
                    case PotionType.HP:
                        if (!DataManager.instance.gameData.hpPotion.Contains(DataManager.instance.gameData.hpPotion.Find(x => x.name == "HP Potion")))
                        {
                            if (uiManager.alert) return;
                            StartCoroutine(uiManager.NoticeText(false, "가방이 가득 찼습니다."));
                            return;
                        }
                        break;
                    case PotionType.SP:
                        if (!DataManager.instance.gameData.spPotion.Contains(DataManager.instance.gameData.spPotion.Find(x => x.name == "SP Potion")))
                        {
                            if (uiManager.alert) return;
                            StartCoroutine(uiManager.NoticeText(false, "가방이 가득 찼습니다."));
                            return;
                        }
                        break;
                }
            }
        }

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
            DataManager.instance.gameData.shortWeapon.Add(uiManager.items[itemIdx] as Weapon);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.longWeapon)
        {
            DataManager.instance.gameData.longWeapon.Add(uiManager.items[itemIdx] as Weapon);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.shoes)
        {
            DataManager.instance.gameData.shoes.Add(uiManager.items[itemIdx] as Clothes);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.top)
        {
            DataManager.instance.gameData.top.Add(uiManager.items[itemIdx] as Clothes);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.bottoms)
        {
            DataManager.instance.gameData.bottoms.Add(uiManager.items[itemIdx] as Clothes);
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
                        DataManager.instance.gameData.hpPotion.Add((Potion)uiManager.items[itemIdx]);
                    break;
                case PotionType.SP:
                    if (DataManager.instance.gameData.spPotion.Contains(DataManager.instance.gameData.spPotion.Find(x => x.name == "SP Potion")))
                    {
                        Potion curPotion = DataManager.instance.gameData.spPotion.Find(x => x.name == "SP Potion") as Potion;
                        curPotion.count += newPotion.count;
                    }
                    else
                        DataManager.instance.gameData.spPotion.Add((Potion)uiManager.items[itemIdx]);
                    break;
            }
            uiManager.items[itemIdx] = null;
        }
        Destroy(gameObject);
    }
}