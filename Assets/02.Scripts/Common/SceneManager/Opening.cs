using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    [SerializeField] GameObject fadeScene;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip btnSfx;

    private void Start()
    {
        source = Camera.main.GetComponent<AudioSource>();
        btnSfx = Resources.Load<AudioClip>("Sound/Recharge Energy1");
    }

    public void GameStart()
    {
        source.PlayOneShot(btnSfx, 1.0f);
        fadeScene.SetActive(true);
    }

    public void Quit()
    {
        source.PlayOneShot(btnSfx, 1.0f);
        Application.Quit();
    }
}
