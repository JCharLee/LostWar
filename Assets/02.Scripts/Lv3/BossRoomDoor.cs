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

    private UIManager uiManager;
    private BasicBehaviour basicBehaviour;

    public string interactionPrompt => prompt;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();

        uiManager = FindObjectOfType<UIManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();
    }

    private void Start()
    {
        prompt = "[F] 문 열기";
        cautionText = "두 개의 카드키를 모두 획득해야 합니다.";
    }

    public bool Action(PlayerInteraction interactor)
    {
        var key = interactor.GetComponent<PlayerInteraction>();
        if (key == null) return false;
        if (key.hasKey1 && key.hasKey2)
        {
            uiManager.moveRoutine = StartCoroutine(DoorOpen());
            return true;
        }
        if (!UIManager.instance.alert)
            StartCoroutine(UIManager.instance.NoticeText(false, cautionText));
        return false;
    }

    IEnumerator DoorOpen()
    {
        uiManager.castRoutine = StartCoroutine(UIManager.instance.InteractionCasting(castTime));
        if (uiManager.castRoutine == null) uiManager.moveRoutine = null;
        yield return new WaitForSeconds(castTime);
        thePlayer.currentMapName = transferMapName;
        SceneLoader.LoadScene(transferMapName);
    }
}
