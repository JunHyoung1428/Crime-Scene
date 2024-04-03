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
    [SerializeField] Image markedImage;
    [SerializeField] public int index;

    [SerializeField] public Screenshot screenshot;
    // Todo ������ path�� index�� �޴°��� �ƴ϶� Screenshot ��ü�� �����ϵ��� �ٲܰ�

    public ScreenshotAlbumUI albumUI;
    
    private void Start()
    {
        image.sprite = Extension.LoadSprite(screenshot.Data.path);
        UpdateMarking();
    }


    public void OnClick()
    {
        albumUI.curIndex = index;
        albumUI.selectedScreenshotImage.sprite = Extension.LoadSprite(screenshot.Data.path);
    }

    public void UpdateMarking()
    {
        markedImage.enabled = screenshot.Data.isBookmarked;
    }
    public void Remove()
    {
        Destroy(gameObject);
    }

}
