using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    /*    public List<PictureData> pictureData;*/
    // ���� ���� ������
    public List<LineData> lineDatas;

    public TutorialData()
    {
        PlayerSubAnswers1 = new List<string>();
        PlayerMultiAnswer = new List<int>();
        lineDatas = new List<LineData>();
    }
}

[Serializable]
public class Chapter1Data
{
    public Vector3 playerPos;
    public Quaternion playerRot;

}
/*[Serializable]
public struct PictureData
{
    public Transform transform;
    public Image image;
}
*/
[Serializable]
public struct LineData
{
    public Color color;
    public int count;
    public Vector3 [] pos;
}