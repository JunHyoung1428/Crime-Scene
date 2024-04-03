using Cinemachine;
using System.Collections;
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
    // ���� ��ư
    [SerializeField] Button submit;

    // �����
    [Header("Answer Sheet")]
    [SerializeField] List<string> subjecttiveAnswers;
    [SerializeField] List<string> multipleChoiceAnswer;

    // �÷��̾� �����
    [Header("Player Answer Sheet")]
    [SerializeField] List<TMP_InputField> PlayerSubAnswers = new List<TMP_InputField>() ;
    [SerializeField] List<TextMeshProUGUI> PlayerMultiAnswer = new List<TextMeshProUGUI>() ;

    public void SubmitButton()
    {
        if( PlayerSubAnswers [0].text == subjecttiveAnswers [0] )
        {

        }
        
    }

    public void OpenAnwer()
    {

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