using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Tutorial
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


    public Tutorial()
    {
        PlayerSubAnswers1 = new List<string>();
        PlayerMultiAnswer = new List<int>();
        Picture = new List<GameObject>();
        lines = new List<GameObject>();
    }
}

[Serializable]
public class Chapter1
{
    public Vector3 playerPos;
    public Quaternion playerRot;

}