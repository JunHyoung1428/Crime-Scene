using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenshotAlbumUI : PopUpUI
{
    // View
    //������� Album UI ������ ó���ϰ� ScreenshotAlbum�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ
    [SerializeField] Image selectedScreenshotImage; //���õ� ��ũ���� �̹���
    [SerializeField] ScreenshotSlotUI ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab
  
    public List<ScreenshotSlotUI> screenshotSlots;
    public ScreenshotSlotUI curSlot;

    [SerializeField] GameObject albumPanel;
    [SerializeField] PopUpUI lookedPanelUI;
    [SerializeField] RectTransform albumGrid;


    private float height;

    bool isActive = false;
    bool isInit = false;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    protected override void Awake()
    {
        base.Awake();
        screenshotSlots = new List<ScreenshotSlotUI>();
        GetUI<Button>("BTN_Look").onClick.AddListener(ButtonLook);
        //InitAlbumUISlots();
    }


  
    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    public void InitAlbumUISlots()
    {
        Debug.Log("Init AlbumUI");
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        if ( count == 0 ) return;

        for ( int i = 0; i < count; i++ )
        {
            ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
            RectTransform rect = slot.GetComponent<RectTransform>();
            slot.Screenshot = ScreenshotAlbum.Instance.Screenshots[i];
            slot.albumUI = this;    
            rect.SetParent(albumGrid);
            rect.localScale = Vector3.one;
            screenshotSlots.Add(slot);
            curSlot = slot;
        }
        SetGridSize(count);
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.Screenshot.Data.path);
        isInit = true;
    }

    public void UpdateAlbumUISlots()
    {
        Debug.Log("Album Slot Update");
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.Screenshot = ScreenshotAlbum.Instance.Screenshots [count - 1];
        slot.albumUI = this;
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
        SetGridSize(count);
    }

    private void SetGridSize(int count )
    {
        height = (count / 3) * 110 + 100;
        albumGrid.sizeDelta = new Vector2(albumGrid.sizeDelta.x, height);
    }

    public void UpdateSelectedImage()
    {
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.Screenshot.Data.path);
    }

    private void DeleteFromAlbum()
    {
       screenshotSlots.Remove(curSlot);
       curSlot.Delete();
       SetGridSize(screenshotSlots.Count);
       curSlot = screenshotSlots [screenshotSlots.Count - 1];
       UpdateSelectedImage();
    }

    public void Active()
    {
        isActive = !isActive;
        albumPanel.SetActive(isActive);

        if ( !isInit ) // ���� ����ÿ��� �ʱ�ȭ
        {
            InitAlbumUISlots();
        }    
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsInit()
    {
        return isInit;
    }

    /***********************************************************************
    *                              OnClick Events
    ***********************************************************************/
    
    public void ButtonDelete()
    {
        //Ȯ���˾� ���°� �߰��ؾ���
        DeleteFromAlbum();
    }

    public void ButtonLook()
    {
        Manager.UI.ShowPopUpUI(lookedPanelUI);
    }

    public void ButtonMarking()
    {
        curSlot.UpdateMarking();
    }

}
