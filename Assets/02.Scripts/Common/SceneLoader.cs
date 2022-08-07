using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image loadingBar;

    public static string nextScene;
    public static bool isLoading = false;

    private void Awake()
    {
        loadingBar = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        loadingBar.fillAmount = 0f;
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        switch(nextScene)
        {
            case "Level2":
                SceneManager.LoadScene("Lv2_LoadingScene");
                break;
            case "Level3":
                SceneManager.LoadScene("Lv3_LoadingScene");
                break;
            case "BossRoom":
                SceneManager.LoadScene("BossRoom_LoadingScene");
                break;
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        isLoading = true;
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, timer * 0.1f);
                if (loadingBar.fillAmount >= op.progress)
                    timer = 0f;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer * 0.1f);
                if (loadingBar.fillAmount == 1.0f)
                {
                    isLoading = false;
                    op.allowSceneActivation = true;
                    DataManager.instance.SaveData();
                    yield break;
                }
            }
        }
    }
}