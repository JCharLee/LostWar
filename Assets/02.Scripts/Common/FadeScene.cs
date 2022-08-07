using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private CanvasGroup fadeCg;
    public string sceneName;
    private float fadeDuration = 3.0f;

    private void Awake()
    {
        fadeObject = this.gameObject;
        fadeCg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        if (fadeCg.alpha == 0)
        {
            float rate = 1.0f / fadeDuration;
            float progress = 0.0f;
            while (progress <= 1.0f)
            {
                fadeCg.alpha = Mathf.Lerp(0f, 1f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            fadeCg.alpha = 1.0f;
            switch(sceneName)
            {
                case "Start":
                    SceneManager.LoadScene("Level1");
                    break;
                case "Finish":
                    Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                    Destroy(FindObjectOfType<UIManager>().gameObject);
                    Destroy(FindObjectOfType<QuestManager>().gameObject);
                    Destroy(FindObjectOfType<TalkManager>().gameObject);
                    Destroy(FindObjectOfType<GameManager>().gameObject);
                    SceneManager.LoadScene("Finish");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case "Main":
                    SceneManager.LoadScene("Main");
                    break;
                case "Retry":
                    if (PlayerInteraction.instance.currentMapName == "Level1")
                    {
                        Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                        Destroy(FindObjectOfType<UIManager>().gameObject);
                        Destroy(FindObjectOfType<QuestManager>().gameObject);
                        Destroy(FindObjectOfType<TalkManager>().gameObject);
                        Destroy(FindObjectOfType<GameManager>().gameObject);
                        SceneManager.LoadScene("Level1");
                    }
                    else
                    {
                        SceneManager.LoadScene(PlayerInteraction.instance.currentMapName);
                        switch (PlayerInteraction.instance.currentMapName)
                        {
                            case "Level2":
                                QuestManager.instance.QuestId = 40;
                                QuestManager.instance.QuestActionIdx = 1;
                                break;
                            case "Level3":
                                QuestManager.instance.QuestId = 80;
                                QuestManager.instance.QuestActionIdx = 1;
                                break;
                            case "BossRoom":
                                QuestManager.instance.QuestId = 130;
                                QuestManager.instance.QuestActionIdx = 1;
                                break;
                        }
                        DataManager.instance.LoadData();
                        QuestManager.instance.Start();
                    }
                    break;
                case "Return":
                    Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                    Destroy(FindObjectOfType<UIManager>().gameObject);
                    Destroy(FindObjectOfType<QuestManager>().gameObject);
                    Destroy(FindObjectOfType<TalkManager>().gameObject);
                    Destroy(FindObjectOfType<GameManager>().gameObject);
                    SceneManager.LoadScene("Main");
                    break;
            }
        }
        else if (fadeCg.alpha == 1)
        {
            float rate = 1.0f / fadeDuration;
            float progress = 0.0f;
            while (progress <= 1f)
            {
                fadeCg.alpha = Mathf.Lerp(1f, 0f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            fadeCg.alpha = 0f;
            fadeObject.SetActive(false);
        }
    }
}
