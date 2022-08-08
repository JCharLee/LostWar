using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    [SerializeField] private int questId;
    private PlayerInteraction player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (DataManager.instance.gameData.questId == questId)
                player.Locate();
        }
    }
}
