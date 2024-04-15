using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerChapter1UI : PopUpUI
{
    [SerializeField] RectTransform ComputerContent;
    Vector2 CreatePoint;
    // ����
    int score = 0;
    int backgound = 1;

    // ����� �߰�
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject answerParents;

    // �����
    [Header("Answer Sheet")]
    [SerializeField] string subjecttiveAnswer1;
    [SerializeField] List<string> subjecttiveAnswers2;
    [SerializeField] string subjecttiveAnswer3;
    [SerializeField] List<string> multipleChoiceAnswer;

    // �÷��̾� �����
    [Header("Player Answer Sheet")]
    [SerializeField] TMP_InputField PlayerSubAnswers1;
    // �ְ����� ������������� ������ ����Ʈ���� �������ְ� ����ؾߵɵ�
    // ��� �� �̴�� �����
    [SerializeField] List<TMP_InputField> PlayerSubAnswers2 = new List<TMP_InputField>();
    [SerializeField] TMP_InputField PlayerSubAnswers3;

    // ������ �ϳ��� �ۿ� ������ ȥ�� ���� �ϸ�ɰŰ���
    [SerializeField] List<TextMeshProUGUI> PlayerMultiAnswer = new List<TextMeshProUGUI>();
    List<GameObject> addedList = new List<GameObject>();

    protected override void Awake()
    {
        Manager.Data.LoadData();
        for ( int i = 0; i < Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Count; i++ )
        {
            if ( i > 0 )
            {
                // ����� �߰�
                GameObject addList = Instantiate(prefab, CreatePoint, Quaternion.identity, answerParents.transform);
                addedList.Add(addList);
                PlayerSubAnswers2.Add(addList.GetComponent<TMP_InputField>());
                // ��׶��� ũ�� �����
                answerParents.GetComponent<RectTransform>().sizeDelta = new Vector2(answerParents.GetComponent<RectTransform>().sizeDelta.x, 70 * backgound);
                // ��ũ�� ũ�� �����
                ComputerContent.sizeDelta = new Vector2(ComputerContent.sizeDelta.x, 250 + 130 * backgound);
                backgound++;
            }
        }
        base.Awake();
        //GetUI<TMP_InputField>("Subjecttive 1").text = "UI Binding Test";
    }
    private void Start()
    {
        Manager.Data.LoadAnswer(PlayerSubAnswers2, PlayerMultiAnswer);
    }
    public void CreateAnswerSheet()
    {
        // ����� �߰�
        addedList.Add(Instantiate(prefab, CreatePoint, Quaternion.identity, answerParents.transform));
        // ��׶��� ũ�� �����
        answerParents.GetComponent<RectTransform>().sizeDelta = new Vector2(answerParents.GetComponent<RectTransform>().sizeDelta.x, 70 * backgound);
        // ��ũ�� ũ�� �����
        ComputerContent.sizeDelta = new Vector2(ComputerContent.sizeDelta.x, 250 + 130 * backgound);

        PlayerSubAnswers2.Add(addedList [0].gameObject.GetComponent<TMP_InputField>());
        backgound++;
    }

    public void ClearAnswerSheet()
    {
        if ( addedList != null )
        {
            Destroy(addedList [0]);
            addedList.RemoveAt(0);
            PlayerSubAnswers2.RemoveAt(1);
            backgound--;
            // ��׶��� ũ�� �����
            answerParents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 70 * backgound);
            // ��ũ�� ũ�� �����
            ComputerContent.sizeDelta = new Vector2(0, 250 + 130 * backgound);
        }
    }

    public void Submit()
    {
        string answer;
        answer = PlayerSubAnswers1.text;
        answer = answer.Replace(" ", string.Empty);
        if ( subjecttiveAnswer1 == answer )
            score++;
        // �ְ��� �� üũ(�߰��Ǵ� ģ��)
        for ( int i = 0; i < subjecttiveAnswers2.Count; i++ )
        {
            for ( int j = 0; j < PlayerSubAnswers2.Count; j++ )
            {
                answer = PlayerSubAnswers2 [j].text;
                answer = answer.Replace(" ", string.Empty);

                if ( subjecttiveAnswers2 [i] == answer )
                {
                    score++;
                }
            }
        }
        answer = PlayerSubAnswers3.text;
        answer = answer.Replace(" ", string.Empty);
        if ( subjecttiveAnswer3 == answer )
            score++;

        // ������ �� üũ
        for ( int i = 0; i < PlayerMultiAnswer.Count; i++ )
        {
            if ( PlayerMultiAnswer [i].text == multipleChoiceAnswer [i] )
            {
                score++;
            }
        }
        // ���� ������ ���� ��ȭ���ָ�ɵ� (����, �� �̵�)
        //Manager.Data.SaveAnswer(PlayerSubAnswers2, PlayerMultiAnswer, score);
        Debug.Log($"������ {score}");

    }
    private void OnDisable()
    {
        //Manager.Data.SaveAnswer(PlayerSubAnswers2, PlayerMultiAnswer, score);
    }

    public void ActivateInputField()
    {
        Manager.Game.ChangeIsChatTrue();
    }
    public void DisableInputField()
    {
        Manager.Game.ChangeIsChatFalse();
    }
}
