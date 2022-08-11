using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContents : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentsText;
    [SerializeField] private Text countText;

    [SerializeField] private QuestData questData;
    public static QuestContents contents;

    public QuestData QuestData
    {
        get
        {
            return questData;
        }
        set
        {
            questData = value;
        }
    }

    private void Awake()
    {
        titleText = transform.GetChild(0).GetComponent<Text>();
        contentsText = transform.GetChild(1).GetComponent<Text>();
        countText = transform.GetChild(2).GetComponent<Text>();

        contents = this;
    }

    private void Start()
    {
        titleText.text = questData.questName;
        if (questData.isMain)
            titleText.color = Color.yellow;
        else
            titleText.color = Color.white;

        contentsText.text = questData.contentsName;
    }

    private void Update()
    {
        countText.text = $"({questData.goal.currentAmount}/{questData.goal.requireAmount})";

        if (questData.goal.IsReached())
        {
            contentsText.color = Color.green;
            countText.color = Color.green;
            Destroy(this.gameObject, 2.0f);
        }
        else
        {
            contentsText.color = Color.white;
            countText.color = Color.white;
        }
    }

    private void OnDisable()
    {
        titleText.text = null;
        contentsText.text = null;
        countText.text = null;
        questData.goal.currentAmount = 0;
    }
}
