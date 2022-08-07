using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private int questId;
    [SerializeField] private int questActionIdx;

    private UIManager uiManager;
    [SerializeField] private QuestGiver questGiver;
    private PlayerInteraction player;
    [SerializeField] private QuestData questData;

    Dictionary<int, QuestData> questList;

    public int QuestId
    {
        get
        {
            return questId;
        }
        set
        {
            questId = value;
        }
    }
    public int QuestActionIdx
    {
        get
        {
            return questActionIdx;
        }
        set
        {
            questActionIdx = value;
        }
    }
    public Dictionary<int, QuestData> QuestList => questList;

    public static QuestManager instance = null;

    private bool isStarting = false;
    public bool IsStarting => isStarting;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        questList = new Dictionary<int, QuestData>();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        GenerateData();
    }

    public void Start()
    {
        isStarting = true;
        StartCoroutine(QuestStart());
    }

    IEnumerator QuestStart()
    {
        yield return new WaitForSeconds(3.0f);
        questGiver.gameObject.SetActive(true);
        uiManager.Action(questGiver.gameObject);
        isStarting = false;
    }

    void GenerateData()
    {
        // 메인 퀘스트
        questList.Add(0, new QuestData("시작", "게임 스타트", new int[] { 0 }, true, 1, GoalType.TALK, 0f));
        questList.Add(10, new QuestData("아파트 탈출", "장비를 찾아서 챙긴다.", new int[] { 0, 0 }, true, 3, GoalType.GATHERING, 10f));
        questList.Add(20, new QuestData("아파트 탈출", "집 밖으로 나가자.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 10f));
        questList.Add(30, new QuestData("아파트 탈출", "출몰하는 적 처치", new int[] { 0, 0 }, true, 10, GoalType.KILL, 30f));
        questList.Add(40, new QuestData("아파트 탈출", "건물을 빠져나간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 10f));

        questList.Add(50, new QuestData("가족의 행방을 찾아서", "캠프로 이동해서 정보를 얻는다.", new int[] { 2000 }, true, 1, GoalType.TALK, 5f));
        questList.Add(60, new QuestData("가족의 행방을 찾아서", "단장에게 말을 건다.", new int[] { 3000 }, true, 1, GoalType.TALK, 5f));
        questList.Add(70, new QuestData("가족의 행방을 찾아서", "적 처치", new int[] { 0, 3000 }, true, 10, GoalType.KILL, 20f));
        questList.Add(80, new QuestData("가족의 행방을 찾아서", "연구소에 들어간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));

        questList.Add(90, new QuestData("연구소 심층부로", "첫 번째 카드키를 찾아서 획득", new int[] { 0, 0 }, true, 1, GoalType.GATHERING, 5f));
        questList.Add(100, new QuestData("연구소 심층부로", "우측방으로 이동", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));
        questList.Add(110, new QuestData("연구소 심층부로", "나타난 적들 처치", new int[] { 0, 0 }, true, 5, GoalType.KILL, 20f));
        questList.Add(120, new QuestData("연구소 심층부로", "두 번째 카드키 획득", new int[] { 0, 0 }, true, 1, GoalType.GATHERING, 5f));
        questList.Add(130, new QuestData("연구소 심층부로", "연구소 심층부에 들어간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 10f));
        questList.Add(140, new QuestData("연구소장 처치", "연구소장 처치", new int[] { 0, 0 }, true, 1, GoalType.KILL, 30f));
        questList.Add(150, new QuestData("", "", new int[] { 0 }, true, 0, GoalType.TALK, 0f));
    }

    public int GetQuestTalkIdx(int id)
    {
        return questId + questActionIdx;
    }

    public void CheckQuest(int id)
    {
        if (id == questList[questId].NpcId[questActionIdx])
        {
            questActionIdx++;
        }

        if (questActionIdx == questList[questId].NpcId.Length)
            NextMainQuest();
    }

    void NextMainQuest()
    {
        uiManager.UpdateExp(questList[questId].ExpReward);
        questId += 10;
        questActionIdx = 0;

        questData = questList[questId];
        player.QuestData = questData;
        questData.isActive = true;
        uiManager.QuestListPanel.SetActive(true);
        if (questId != 150)
        {
            Instantiate(uiManager.QuestPrefab, uiManager.QuestListPanel.transform);
            QuestContents.contents.QuestData = questData;
        }
        else
            uiManager.fadeObject.SetActive(true);
    }

    public void Complete()
    {
        if (questId == 110)
        {
            GameObject key = Instantiate(Resources.Load<GameObject>("Prefabs/SecondCardKey"), new Vector3(42f, -55.34903f, 57f), Quaternion.Euler(0f, 0f, 25f));
            key.GetComponent<Key>().keyNumber = 2;
        }

        questData.isActive = false;
        if (questList[questId].NpcId.Length == 1) return;

        if (questList[questId].NpcId[questActionIdx + 1] == 0)
        {
            questActionIdx++;
            questGiver.gameObject.SetActive(true);
            uiManager.Action(questGiver.gameObject);
        }
        else
            questActionIdx++;
    }
}