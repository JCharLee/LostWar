using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    [SerializeField] GameObject fadeScene;

    public void GameStart()
    {
        fadeScene.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
