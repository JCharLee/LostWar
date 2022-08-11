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
                    SceneManager.LoadScene("Finish");
                    Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                    Destroy(FindObjectOfType<UIManager>().gameObject);
                    Destroy(FindObjectOfType<QuestManager>().gameObject);
                    Destroy(FindObjectOfType<TalkManager>().gameObject);
                    Destroy(FindObjectOfType<GameManager>().gameObject);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case "Main":
                    SceneManager.LoadScene("Main");
                    break;
                case "Retry":
                    if (PlayerInteraction.instance.currentMapName == "Level1")
                    {
                        SceneManager.LoadScene("Level1");
                        Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                        Destroy(FindObjectOfType<UIManager>().gameObject);
                        Destroy(FindObjectOfType<QuestManager>().gameObject);
                        Destroy(FindObjectOfType<TalkManager>().gameObject);
                        Destroy(FindObjectOfType<GameManager>().gameObject);
                    }
                    else
                    {
                        SceneManager.LoadScene(PlayerInteraction.instance.currentMapName);
                        DataManager.instance.LoadData();
                        DataManager.instance.gameData.questActionIdx++;
                        if (UIManager.instance.QuestListPanel.transform.childCount != 0)
                            Destroy(FindObjectOfType<QuestContents>().gameObject);
                        UIManager.instance.GameOverPanelOff();
                        QuestManager.instance.Start();
                        StartCoroutine(Fade());
                    }
                    break;
                case "Return":
                    SceneManager.LoadScene("Main");
                    Destroy(FindObjectOfType<PlayerInteraction>().gameObject);
                    Destroy(FindObjectOfType<UIManager>().gameObject);
                    Destroy(FindObjectOfType<QuestManager>().gameObject);
                    Destroy(FindObjectOfType<TalkManager>().gameObject);
                    Destroy(FindObjectOfType<GameManager>().gameObject);
                    break;
            }
            if (sceneName != "Start" && sceneName != "Finish")
            {
                FindObjectOfType<Health>().ani.SetTrigger("Awake");
                FindObjectOfType<Health>().ani.SetFloat("Speed", 0f);
                FindObjectOfType<Health>().isdie = false;
            }
            sceneName = "Finish";
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
