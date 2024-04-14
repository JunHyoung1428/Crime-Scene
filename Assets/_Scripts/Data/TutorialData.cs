using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TutorialData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    // �ְ��� �÷��̾� ��
    public List<string> PlayerSubAnswers1;
    // ������ �÷��̾� ��
    public List<int> PlayerMultiAnswer;
    // Ʃ�丮�� ���ھ�
    public int tutorialScore;

    // ���� ���� ������
    public List<GameObject> Picture;

    public List<GameObject> lines;


    public TutorialData()
    {
        PlayerSubAnswers1 = new List<string>();
        PlayerMultiAnswer = new List<int>();
        Picture = new List<GameObject>();
        lines = new List<GameObject>();
    }
}

[Serializable]
public class Chapter1Data
{
    public Vector3 playerPos;
    public Quaternion playerRot;

}