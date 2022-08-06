using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalType
{
    KILL, GATHERING, TALK, LOCATION
}

[System.Serializable]
public class QuestGoal
{
    [SerializeField] private GoalType goalType;

    [SerializeField] private int currentAmount = 0;
    [SerializeField] private int requireAmount;

    public GoalType GoalType => goalType;
    public int CurrentAmount => currentAmount;
    public int RequireAmount => requireAmount;

    public QuestGoal(GoalType type, int reqAmount)
    {
        goalType = type;
        requireAmount = reqAmount;
    }

    public bool IsReached()
    {
        return (currentAmount >= requireAmount);
    }

    public void EnemyKilled()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, requireAmount);
        if (goalType == GoalType.KILL)
            currentAmount++;
    }

    public void ItemCollected()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, requireAmount);
        if (goalType == GoalType.GATHERING)
            currentAmount++;
    }

    public void Talking()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, requireAmount);
        if (goalType == GoalType.TALK)
            currentAmount++;
    }

    public void Locate()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, requireAmount);
        if (goalType == GoalType.LOCATION)
            currentAmount++;
    }
}
