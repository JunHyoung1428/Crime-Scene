using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : PopUpUI
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
    [SerializeField] List<string> subjecttiveAnswers1;
    [SerializeField] List<string> multipleChoiceAnswer;

    // �÷��̾� �����
    [Header("Player Answer Sheet")]
    // �ְ����� ������������� ������ ����Ʈ���� �������ְ� ����ؾߵɵ�
    [SerializeField] List<TMP_InputField> PlayerSubAnswers1 = new List<TMP_InputField>();
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
                PlayerSubAnswers1.Add(addList.GetComponent<TMP_InputField>());
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
    public void Start()
    {
        // ���� �ҷ�����
        for ( int i = 0; i < Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Count; i++ )
            PlayerSubAnswers1 [i].text = Manager.Data.GameData.tutorialData.PlayerSubAnswers1 [i];
        for ( int i = 0; i < Manager.Data.GameData.tutorialData.PlayerMultiAnswer.Count; i++ )
            PlayerMultiAnswer [i].GetComponentInParent<TMP_Dropdown>().value = Manager.Data.GameData.tutorialData.PlayerMultiAnswer [i];
    }
    public void CreateAnswerSheet()
    {
        // ����� �߰�
        addedList.Add(Instantiate(prefab, CreatePoint, Quaternion.identity, answerParents.transform));
        // ��׶��� ũ�� �����
        answerParents.GetComponent<RectTransform>().sizeDelta = new Vector2(answerParents.GetComponent<RectTransform>().sizeDelta.x, 70 * backgound);
        // ��ũ�� ũ�� �����
        ComputerContent.sizeDelta = new Vector2(ComputerContent.sizeDelta.x, 250 + 130 * backgound);

        PlayerSubAnswers1.Add(addedList [0].gameObject.GetComponent<TMP_InputField>());
        backgound++;
    }

    public void ClearAnswerSheet()
    {
        if ( addedList != null )
        {
            Destroy(addedList [0]);
            addedList.RemoveAt(0);
            PlayerSubAnswers1.RemoveAt(1);
            backgound--;
            // ��׶��� ũ�� �����
            answerParents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 70 * backgound);
            // ��ũ�� ũ�� �����
            ComputerContent.sizeDelta = new Vector2(0, 250 + 130 * backgound);
        }
    }

    public void Submit()
    {
        // �ְ��� �� üũ
        for ( int i = 0; i < subjecttiveAnswers1.Count; i++ )
        {
            for ( int j = 0; j < PlayerSubAnswers1.Count; j++ )
            {
                string answer = PlayerSubAnswers1 [j].text;
                answer = answer.Replace(" ", string.Empty);

                if ( subjecttiveAnswers1 [i] == answer )
                {
                    score++;
                }
            }
        }
        // ������ �� üũ
        for ( int i = 0; i < PlayerMultiAnswer.Count; i++ )
        {
            if ( PlayerMultiAnswer [i].text == multipleChoiceAnswer [i] )
            {
                score++;
            }
        }
        // ���� ������ ���� ��ȭ���ָ�ɵ� (����, �� �̵�)
        Manager.Data.SaveData();
        Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Clear();
        Manager.Data.GameData.tutorialData.PlayerMultiAnswer.Clear();
        // �ְ��� ����
        for ( int i = 0; i < PlayerSubAnswers1.Count; i++ )
        {
            Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Add(PlayerSubAnswers1 [i].text);
        }
        // ������ ����
        for ( int i = 0; i < PlayerMultiAnswer.Count; i++ )
        {
            Manager.Data.GameData.tutorialData.PlayerMultiAnswer.Add(PlayerMultiAnswer [i].GetComponentInParent<TMP_Dropdown>().value);
        }
        Manager.Data.GameData.tutorialData.tutorialScore = score;
        Debug.Log($"������ {score}");


    }
    private void OnDisable()
    {
        Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Clear();
        Manager.Data.GameData.tutorialData.PlayerMultiAnswer.Clear();
        // �ְ��� ����
        for ( int i = 0; i < PlayerSubAnswers1.Count; i++ )
        {
            Manager.Data.GameData.tutorialData.PlayerSubAnswers1.Add(PlayerSubAnswers1 [i].text);
        }
        // ������ ����
        for ( int i = 0; i < PlayerMultiAnswer.Count; i++ )
        {
            Manager.Data.GameData.tutorialData.PlayerMultiAnswer.Add(PlayerMultiAnswer [i].GetComponentInParent<TMP_Dropdown>().value);
        }
        Manager.Data.SaveData();
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
