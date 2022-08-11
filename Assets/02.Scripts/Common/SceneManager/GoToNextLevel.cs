using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    [SerializeField] private string transferMapName;
    [SerializeField] private int questId;
    private string alert;
    private PlayerInteraction thePlayer;

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerInteraction>();
        alert = "퀘스트를 더 진행해야 합니다.";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (DataManager.instance.gameData.questId < questId)
            {
                if (UIManager.instance.alert) return;
                StartCoroutine(UIManager.instance.NoticeText(false, alert));
                return;
            }
            thePlayer.currentMapName = transferMapName;
            SceneLoader.LoadScene(transferMapName);
        }
    }
}