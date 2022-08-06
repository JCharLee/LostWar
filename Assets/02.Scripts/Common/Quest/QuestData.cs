using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    [SerializeField] private string questName;
    [SerializeField] private string contentsName;
    private int[] npcId;
    [SerializeField] private bool isMain;
    [SerializeField] private QuestGoal goal;
    private float expReward;

    public bool isActive;

    public string QuestName => questName;
    public string ContentsName => contentsName;
    public int[] NpcId => npcId;
    public bool IsMain => isMain;
    public QuestGoal Goal => goal;
    public float ExpReward => expReward;

    public QuestData(string name, string content, int[] npc, bool main, int amount, GoalType type, float exp)
    {
        questName = name;
        contentsName = content;
        npcId = npc;
        isMain = main;
        goal = new QuestGoal(type, amount);
        expReward = exp;
    }
}