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

    ElevatorDoor topDoor;
    ElevatorDoor bottomDoor;
    UIManager uiManager;
    BasicBehaviour basicBehaviour;

    [SerializeField] string prompt;

    void Awake()
    {
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

    void Start()
    {
        mr.material = mat[0];
        _light.color = Color.red;

        moveTime = (topFloor.position - bottomFloor.position).magnitude / speed;
        prompt = "[F] 엘레베이터 작동";
    }

    public string interactionPrompt => prompt;

    public bool Action(PlayerInteraction interactor)
    {
        isDown = !isDown;
        uiManager.moveRoutine = StartCoroutine(ElevatorMove());
        return true;
    }

    IEnumerator ElevatorMove()
    {
        current = 0f;

        if (onElevator)
        {
            uiManager.castRoutine = StartCoroutine(uiManager.InteractionCasting(castingTime));
            if (uiManager.castRoutine == null) uiManager.moveRoutine = null;
            yield return new WaitForSeconds(castingTime);
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