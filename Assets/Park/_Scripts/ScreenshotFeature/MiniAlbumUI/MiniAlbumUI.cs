using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAlbumUI : MonoBehaviour
{
    [SerializeField] MiniSlotUI ScreenshotSlotUIPrefab; //스크린샷 슬롯 UI Prefab

    public List<MiniSlotUI> screenshotSlots;

    [SerializeField] GameObject albumPanel;
    [SerializeField] RectTransform albumGrid;



    ScreenshotAlbum album;
    private float height;

    bool isActive = false;
    bool isInit = false;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    protected void Awake()
    {
        screenshotSlots = new List<MiniSlotUI>();
    }

    private void Start()
    {
        album = ScreenshotAlbum.Instance;
        album.OnScreenshotDeleted += OnScreenshotDeleted;
        album.OnScreenshotAdd += UpdateAlbumUISlots;
        InitAlbumUISlots();
    }


    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    public void InitAlbumUISlots()
    {
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        for ( int i = 0; i < count; i++ )
        {
            MiniSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
            RectTransform rect = slot.GetComponent<RectTransform>();
            slot.Screenshot = ScreenshotAlbum.Instance.Screenshots [i];
            rect.SetParent(albumGrid, false);
            rect.localScale = Vector3.one;
            screenshotSlots.Add(slot);
        }
        SetGridSize(count);
        isInit = true;
    }

    public void UpdateAlbumUISlots(Screenshot screenshot)
    {
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        MiniSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.Screenshot = ScreenshotAlbum.Instance.Screenshots [count - 1];
        rect.SetParent(albumGrid, false);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
        SetGridSize(count);
    }

    public void OnScreenshotDeleted( Screenshot screenshot )
    {
        MiniSlotUI slotUI = screenshotSlots.Find(s => s.Screenshot.Data.path == screenshot.Data.path); 
        if ( slotUI != null )
        {
            screenshotSlots.Remove(slotUI);
            slotUI.Delete();
        }
        SetGridSize(ScreenshotAlbum.Instance.Screenshots.Count);
    }

    private void SetGridSize( int count )
    {
        height = ( count ) * 110 + 100;
        albumGrid.sizeDelta = new Vector2(albumGrid.sizeDelta.x, height);
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
}
