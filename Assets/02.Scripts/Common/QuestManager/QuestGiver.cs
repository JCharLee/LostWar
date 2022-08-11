using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteraction
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private int objectID = 0;

    public IInteraction interactable;
    private UIManager uiManager;

    public int ObjectID => objectID;

    public string interactionPrompt => null;

    public Coroutine CastRoutine => throw new System.NotImplementedException();

    public Coroutine MoveRoutine => throw new System.NotImplementedException();

    public float CastingTime => throw new System.NotImplementedException();

    private void Awake()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        interactable = this;
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    public bool Action(PlayerInteraction interactor)
    {
        uiManager.Action(this.gameObject);
        return true;
    }
}
