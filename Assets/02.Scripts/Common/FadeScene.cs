using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private CanvasGroup fadeCg;
    [SerializeField] private string sceneName;
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
