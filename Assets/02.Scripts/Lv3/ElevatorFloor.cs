using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorFloor : MonoBehaviour
{
    [SerializeField] Elevator elevator;
    [SerializeField] ElevatorDoor topDoor;
    [SerializeField] ElevatorDoor bottomDoor;

    void Awake()
    {
        elevator = GameObject.FindGameObjectWithTag("ELEVATOR").transform.GetChild(0).GetComponent<Elevator>();
        topDoor = GameObject.Find("Elevator").transform.GetChild(2).GetComponent<ElevatorDoor>();
        bottomDoor = GameObject.Find("Elevator").transform.GetChild(3).GetComponent<ElevatorDoor>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            elevator.onElevator = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            elevator.onElevator = false;
            StartCoroutine(CloseElevatorDoor());
        }
    }

    IEnumerator CloseElevatorDoor()
    {
        yield return new WaitForSeconds(3.0f);
        
        topDoor.isOpen = false;
        bottomDoor.isOpen = false;
        StartCoroutine(topDoor.DoorAction());
        StartCoroutine(bottomDoor.DoorAction());
    }
}
