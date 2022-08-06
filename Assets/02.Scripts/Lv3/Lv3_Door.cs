using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3_Door : MonoBehaviour, IInteraction
{
    [SerializeField] protected GameObject door;
    [SerializeField] protected Transform doorTr;
    protected Transform doorUp;
    protected Transform doorDown;
    public bool isOpen = false;

    public float speed { get; protected set; }
    public float moveTime { get; protected set; }
    public float current { get; protected set; }
    public string prompt { get; protected set; }

    public virtual string interactionPrompt => prompt;

    protected virtual void Awake()
    {
        door = transform.GetChild(1).gameObject;
        doorTr = door.GetComponent<Transform>();
        doorUp = transform.GetChild(3).GetComponent<Transform>();
        doorDown = transform.GetChild(4).GetComponent<Transform>();
    }
    protected virtual void Start()
    {
        moveTime = (doorUp.position - doorDown.position).magnitude / speed;
    }

    public virtual bool Action(PlayerInteraction interactor)
    {
        isOpen = !isOpen;
        StartCoroutine(DoorAction());
        return true;
    }

    public virtual IEnumerator DoorAction()
    {
        current = 0f;

        if (isOpen)
        {
            if (doorTr.position != doorUp.position)
            {
                this.gameObject.layer = 0;
                while (current <= moveTime)
                {
                    current += Time.deltaTime;
                    doorTr.position = Vector3.MoveTowards(doorTr.position, doorUp.position, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        else
        {
            if (doorTr.position != doorDown.position && !isOpen)
            {
                this.gameObject.layer = 8;
                while (current <= moveTime)
                {
                    current += Time.deltaTime;
                    doorTr.position = Vector3.MoveTowards(doorTr.position, doorDown.position, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}
