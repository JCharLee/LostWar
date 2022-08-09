using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretWall : MonoBehaviour, IInteraction
{
    [SerializeField] Transform secretWall;
    [SerializeField] Transform openPoint;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float moveTime;
    [SerializeField] private float current;
    private float castingTime = 3.0f;

    string prompt;

    private UIManager uiManager;
    private BasicBehaviour basicBehaviour;
    private AudioSource sdooraudio;
    private AudioClip sdooropen;

    void Start()
    {
        secretWall = GetComponent<Transform>();
        openPoint = GameObject.Find("OpenPoint").GetComponent<Transform>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();

        moveTime = (openPoint.position - secretWall.position).magnitude / speed;
        prompt = "[F] 조사하기";

        sdooraudio = GetComponent<AudioSource>();
        sdooropen = Resources.Load<AudioClip>("Sound/SlideDoorOpen");
    }

    public string interactionPrompt => prompt;

    public bool Action(PlayerInteraction interactor)
    {
        uiManager.moveRoutine = StartCoroutine(WallOpen());
        return true;
    }

    IEnumerator WallOpen()
    {
        uiManager.castRoutine = StartCoroutine(uiManager.InteractionCasting(castingTime));
        if (uiManager.castRoutine == null) uiManager.moveRoutine = null;
        yield return new WaitForSeconds(castingTime);
        this.gameObject.layer = 0;

        current = 0f;
        while (current <= moveTime)
        {
            if (current == 0)
                sdooraudio.PlayOneShot(sdooropen, 1.0f);
            current += Time.deltaTime;
            secretWall.position = Vector3.MoveTowards(secretWall.position, openPoint.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}