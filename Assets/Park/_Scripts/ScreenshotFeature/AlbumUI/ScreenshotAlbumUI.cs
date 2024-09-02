using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotAlbumUI : PopUpUI
{
    // View
    //사용자의 Album UI 조작을 처리하고 ScreenshotAlbum과 상호작용 하는 스크립트
    [SerializeField] Image selectedScreenshotImage; //선택된 스크린샷 이미지
    [SerializeField] ScreenshotSlotUI ScreenshotSlotUIPrefab; //스크린샷 슬롯 UI Prefab

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
        ScreenshotAlbum.Instance.OnScreenshotAdd += UpdateAlbumUISlots;
        InitAlbumUISlots();
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
            AddScreenshotSlotUI(slot, i);
        }

        SetGridSize(count);
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.Screenshot.Data.path);
        isInit = true;
    }

    public void UpdateAlbumUISlots(Screenshot screenshot)
    {
        Debug.Log("Album Slot Update");
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        AddScreenshotSlotUI(slot, count - 1);
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.Screenshot.Data.path);
        SetGridSize(count);
    }

    private void AddScreenshotSlotUI( ScreenshotSlotUI slot, int index )
    {
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.Screenshot = ScreenshotAlbum.Instance.Screenshots [index];
        slot.albumUI = this;
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
        curSlot = slot;
    }

    private void SetGridSize( int count )
    {
        height = ( count / 3 ) * 110 + 100;
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
        int cnt = screenshotSlots.Count;
        SetGridSize(cnt);
        if ( cnt == 0 )
        {
            selectedScreenshotImage.sprite = null;
            return;
        }
        curSlot = screenshotSlots [cnt - 1];
        UpdateSelectedImage();
    }

    public void Active()
    {
        isActive = !isActive;
        albumPanel.SetActive(isActive);

        if ( !isInit ) // 최초 실행시에만 초기화
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
        //확인팝업 묻는거 추가해야함
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
