using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerController : MonoBehaviour, IAnswerable, IZoomable
{
    // ��ǻ�Ϳ� �ܵǴ� ī�޶�
    [SerializeField] CinemachineVirtualCamera computerCamera;
    // ��ǻ�Ϳ� ���� ui
    [SerializeField] Canvas canvas;
    Vector2 CreatePoint;
    // ����
    int score = 0;

    // ����� �߰�
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject answerParents;
    // �����
    [Header("Answer Sheet")]
    [SerializeField] List<string> subjecttiveAnswers;
    [SerializeField] List<string> multipleChoiceAnswer;

    // �÷��̾� �����
    [Header("Player Answer Sheet")]
    [SerializeField] List<TMP_InputField> PlayerSubAnswers = new List<TMP_InputField>();
    [SerializeField] List<TextMeshProUGUI> PlayerMultiAnswer = new List<TextMeshProUGUI>();

    [SerializeField] List<GameObject> addedList = new List<GameObject>();
    public void CreateAnswerSheet()
    {
        addedList.Add(Instantiate(prefab, CreatePoint, Quaternion.identity, answerParents.transform));
        PlayerSubAnswers.Add(addedList [0].gameObject.GetComponent<TMP_InputField>());
    }

    public void ClearAnswerSheet()
    {
        if ( addedList != null )
        {
            Destroy(addedList [0]);
            addedList.RemoveAt(0);
        }
    }
    public void SubmitButton()
    {
        // �ְ��� �� üũ
        for ( int i = 0; i < PlayerSubAnswers.Count; i++ )
        {
            string answer = PlayerSubAnswers [i].text;
            Debug.Log(answer);
            answer = answer.Replace(" ", string.Empty);
            if ( answer == subjecttiveAnswers [i] )
            {
                score++;
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
        // ���� ������ ���� ��ȭ���ָ�ɵ�
        Debug.Log($"������ {score}");

    }

    public void UnzoomObject( Transform ZoomTrans )
    {
        computerCamera.m_Priority = 0;
        canvas.enabled = false;
    }

    public void ZoomObject( Transform ZoomTrans )
    {
        computerCamera.m_Priority = 20;
        canvas.enabled = true;
    }


}