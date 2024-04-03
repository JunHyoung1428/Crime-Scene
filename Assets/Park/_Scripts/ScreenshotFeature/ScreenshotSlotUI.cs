using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotSlotUI : MonoBehaviour
{
    //Album UI���� �� ��ũ���� ����

    // ������ �ε��� ����, ������ Screenshot �̹���, ���̶���Ʈ, Ŭ���� SelectedScreenshot UI�� �̹����� ��ü
    [SerializeField] Image image;
    [SerializeField] public string path;


    private void Start()
    {
        image.sprite = Extension.LoadSprite(path);
    }
}
