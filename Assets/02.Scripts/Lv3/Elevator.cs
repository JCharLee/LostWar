using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IInteraction
{
    [SerializeField] Transform elevatorFloor;
    [SerializeField] Transform topFloor;
    [SerializeField] Transform bottomFloor;

    [SerializeField] MeshRenderer mr;
    [SerializeField] Material[] mat;
    [SerializeField] Light _light;

    [SerializeField] private float speed = 10f;
    [SerializeField] float current;
    [SerializeField] float moveTime;
    [SerializeField] float castingTime = 1f;

    private bool isDown = false;
    public bool onElevator = false;

    private IInteraction interactable;
    private Coroutine castRoutine;
    private Coroutine moveRoutine;

    ElevatorDoor topDoor;
    ElevatorDoor bottomDoor;
    UIManager uiManager;
    BasicBehaviour basicBehaviour;

    [SerializeField] string prompt;

    void Awake()
    {
        interactable = this;

        elevatorFloor = GameObject.Find("Elevator").transform.GetChild(1).transform;
        topFloor = GameObject.Find("Elevator").transform.GetChild(5).transform;
        bottomFloor = GameObject.Find("Elevator").transform.GetChild(6).transform;

        mr = transform.GetChild(1).GetComponent<MeshRenderer>();
        mat = mr.materials;
        _light = mr.GetComponentInChildren<Light>();

        topDoor = GameObject.Find("Elevator").transform.GetChild(2).GetComponent<ElevatorDoor>();
        bottomDoor = GameObject.Find("Elevator").transform.GetChild(3).GetComponent<ElevatorDoor>();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();
    }

    public string interactionPrompt => prompt;
    public Coroutine CastRoutine => castRoutine;
    public Coroutine MoveRoutine => moveRoutine;
    float IInteraction.CastingTime => castingTime;

    void Start()
    {
        mr.material = mat[0];
        _light.color = Color.red;

        moveTime = (topFloor.position - bottomFloor.position).magnitude / speed;
        prompt = "[F] 엘레베이터 작동";
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
        isDown = !isDown;
        moveRoutine = StartCoroutine(ElevatorMove());
        return true;
    }

    IEnumerator ElevatorMove()
    {
        current = 0f;

        if (onElevator)
        {
            castRoutine = StartCoroutine(uiManager.InteractionCasting(castingTime));
            yield return new WaitForSeconds(castingTime);
            castRoutine = null;
            moveRoutine = null;
            if (isDown)
            {
                if (elevatorFloor.position != bottomFloor.position)
                {
                    topDoor.isOpen = false;
                    StartCoroutine(topDoor.DoorAction());
                    mr.material = mat[1];
                    _light.color = Color.green;
                    
                    while (current <= moveTime)
                    {
                        this.gameObject.layer = 0;
                        current += Time.deltaTime;
                        elevatorFloor.position = Vector3.MoveTowards(elevatorFloor.position, bottomFloor.position, speed * Time.deltaTime);
                        yield return null;
                    }

                    bottomDoor.isOpen = true;
                    StartCoroutine(bottomDoor.DoorAction());
                    mr.material = mat[0];
                    _light.color = Color.red;
                    this.gameObject.layer = 8;
                }
            }
            else
            {
                if (elevatorFloor.position != topFloor.position)
                {
                    bottomDoor.isOpen = false;
                    StartCoroutine(bottomDoor.DoorAction());
                    mr.material = mat[1];
                    _light.color = Color.green;

                    while (current <= moveTime)
                    {
                        this.gameObject.layer = 0;
                        current += Time.deltaTime;
                        elevatorFloor.position = Vector3.MoveTowards(elevatorFloor.position, topFloor.position, speed * Time.deltaTime);
                        yield return null;
                    }

                    topDoor.isOpen = true;
                    StartCoroutine(topDoor.DoorAction());
                    mr.material = mat[0];
                    _light.color = Color.red;
                    this.gameObject.layer = 8;
                }
            }
        }
    }
}