using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private string startPoint;
    private PlayerInteraction thePlayer;

    public void Start()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();
        if (startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = this.transform.position;
            thePlayer.transform.rotation = this.transform.rotation;
        }
    }
}