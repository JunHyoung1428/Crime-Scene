using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAlbumUI : MonoBehaviour
{
    [SerializeField] MiniSlotUI ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab

    public List<MiniSlotUI> screenshotSlots;

    [SerializeField] GameObject albumPanel;
    [SerializeField] RectTransform albumGrid;


    private float height;

    bool isActive = false;
    bool isInit = false;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    protected void Awake()
    {
        screenshotSlots = new List<MiniSlotUI>();
        InitAlbumUISlots();
    }



    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    //�׸��� ���� ScreenshotSlotUI�� �������� ����

    // ToDo ���� ����/������ ScrollView Content(album Grid)�� Height�� �������� �������ִ� ��
    private void InitAlbumUISlots()
    {
        Debug.Log("�ٹ� �ʱ�ȭ");
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        for ( int i = 0; i < count; i++ )
        {
            MiniSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
            RectTransform rect = slot.GetComponent<RectTransform>();
            slot.screenshot = ScreenshotAlbum.Instance.Screenshots [i];
            rect.SetParent(albumGrid);
            rect.localScale = Vector3.one;
            screenshotSlots.Add(slot);
        }
        SetGridSize(count);
    }

    public void UpdateAlbumUISlots()
    {
        Debug.Log("�ٹ� ������Ʈ");
        int count = ScreenshotAlbum.Instance.Screenshots.Count;
        MiniSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.screenshot = ScreenshotAlbum.Instance.Screenshots [count - 1];
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
        SetGridSize(count);
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

        if ( !isInit ) // ���� ����ÿ��� �ʱ�ȭ
        {
            isInit = true;
            InitAlbumUISlots();
        }
    }
}
