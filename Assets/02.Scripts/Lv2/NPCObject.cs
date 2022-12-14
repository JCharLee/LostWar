using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCObject : MonoBehaviour, IInteraction
{
    public int objectId;
    public string npcName;
    public string prompt;
    public bool hasQuest;
    [SerializeField] private Text questMark;

    private UIManager uiManager;
    private PlayerInteraction player;

    protected virtual void Awake()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        player = FindObjectOfType<PlayerInteraction>();
        prompt = "[F] 대화하기";
    }

    private void Start()
    {
        questMark.enabled = false;
    }

    private void Update()
    {
        if (QuestManager.instance.QuestList[DataManager.instance.gameData.questId].npcId[DataManager.instance.gameData.questActionIdx] == objectId)
            questMark.enabled = true;
        else
            questMark.enabled = false;
    }

    public virtual string interactionPrompt => prompt;

    public Coroutine CastRoutine => throw new System.NotImplementedException();

    public Coroutine MoveRoutine => throw new System.NotImplementedException();

    public float CastingTime => throw new System.NotImplementedException();

    public virtual bool Action(PlayerInteraction interactor)
    {
        uiManager.Action(this.gameObject);
        if (QuestManager.instance.QuestList[DataManager.instance.gameData.questId].npcId[DataManager.instance.gameData.questActionIdx] == objectId)
            player.Talk();
        return true;
    }
}