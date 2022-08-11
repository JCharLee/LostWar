using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string questName;
    public string contentsName;
    public int[] npcId;
    public bool isMain;
    public QuestGoal goal;
    public int expReward;

    public bool isActive;

    public QuestData(string name, string content, int[] npc, bool main, int amount, GoalType type, int exp)
    {
        questName = name;
        contentsName = content;
        npcId = npc;
        isMain = main;
        goal = new QuestGoal(type, amount);
        expReward = exp;
    }
}