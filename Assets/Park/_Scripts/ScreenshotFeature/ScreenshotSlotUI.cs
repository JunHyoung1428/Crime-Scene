using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotSlotUI : MonoBehaviour
{
    //Album UI���� �� ��ũ���� ����

    // ������ �ε��� ����, ������ Screenshot �̹���, ���̶���Ʈ

    [SerializeField] Image image;
    [SerializeField] public string path;
    [SerializeField] public int index;

    public ScreenshotAlbumUI albumUI;
    
    private void Start()
    {
        image.sprite = Extension.LoadSprite(path);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void OnClick()
    {
        albumUI.curSlotIndex = index;
        albumUI.selectedScreenshotImage.sprite = Extension.LoadSprite(path);
    }
}
