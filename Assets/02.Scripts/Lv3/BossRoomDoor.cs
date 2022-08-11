using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour, IInteraction
{
    [SerializeField] private string transferMapName;

    private PlayerInteraction thePlayer;
    private float castTime = 5.0f;
    private string prompt;
    private string cautionText;

    private IInteraction interactable;
    private Coroutine castRoutine;
    private Coroutine moveRoutine;

    private UIManager uiManager;
    private BasicBehaviour basicBehaviour;

    public string interactionPrompt => prompt;
    public Coroutine CastRoutine => castRoutine;
    public Coroutine MoveRoutine => moveRoutine;
    public float CastingTime => castTime;

    private void Awake()
    {
        interactable = this;

        thePlayer = FindObjectOfType<PlayerInteraction>();

        uiManager = FindObjectOfType<UIManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();
    }

    private void Start()
    {
        prompt = "[F] 문 열기";
        cautionText = "두 개의 카드키를 모두 획득해야 합니다.";
    }

    private void Update()
    {
        if (interactable != PlayerInteraction.instance.interactable) return;

        if (UIManager.instance.casting)
        {
            if (basicBehaviour.IsMoving())
            {
                StopCoroutine(castRoutine);
                StopCoroutine(moveRoutine);
                UIManager.instance.StopCasting();
                castRoutine = null;
                moveRoutine = null;

                StartCoroutine(UIManager.instance.NoticeText(false, "중간에 움직여서 취소됐습니다."));
            }
        }
    }

    public bool Action(PlayerInteraction interactor)
    {
        var key = interactor.GetComponent<PlayerInteraction>();
        if (key == null) return false;
        if (key.hasKey1 && key.hasKey2)
        {
            moveRoutine = StartCoroutine(DoorOpen());
            return true;
        }
        if (!UIManager.instance.alert)
            StartCoroutine(UIManager.instance.NoticeText(false, cautionText));
        return false;
    }

    IEnumerator DoorOpen()
    {
        castRoutine = StartCoroutine(UIManager.instance.InteractionCasting(castTime));
        yield return new WaitForSeconds(castTime);
        castRoutine = null;
        moveRoutine = null;
        thePlayer.currentMapName = transferMapName;
        SceneLoader.LoadScene(transferMapName);
    }
}
