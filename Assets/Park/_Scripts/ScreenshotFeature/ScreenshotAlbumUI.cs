using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenshotAlbumUI : MonoBehaviour
{
    // View
    //������� Album UI ������ ó���ϰ� ScreenshotAlbum�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ
    [SerializeField] Image selectedScreenshotImage; //���õ� ��ũ���� �̹���
    [SerializeField] ScreenshotSlotUI ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab
  
    public List<ScreenshotSlotUI> screenshotSlots;
    public ScreenshotSlotUI curSlot;

    [SerializeField] GameObject albumPanel;
    [SerializeField] GameObject lookedPanel;
    [SerializeField] Transform albumGrid;
   
    
    
    bool isActive = false;
    bool isInit = false;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    private void Awake()
    {
        screenshotSlots = new List<ScreenshotSlotUI>();
    }


  
    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    //�׸��� ���� ScreenshotSlotUI�� �������� ����
    private void InitAlbumUISlots()
    {
        Debug.Log("�ٹ� �ʱ�ȭ");
        for ( int i = 0; i < ScreenshotAlbum.Instance.Screenshots.Count; i++ )
        {
            ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
            RectTransform rect = slot.GetComponent<RectTransform>();
            slot.screenshot = ScreenshotAlbum.Instance.Screenshots[i];
            slot.albumUI = this;    
            rect.SetParent(albumGrid);
            rect.localScale = Vector3.one;
            screenshotSlots.Add(slot);
            curSlot = slot;
        }
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.screenshot.Data.path);
    }

    public void UpdateAlbumUISlots()
    {
        Debug.Log("�ٹ� ������Ʈ");
        ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.screenshot = ScreenshotAlbum.Instance.Screenshots[ScreenshotAlbum.Instance.Screenshots.Count-1];
        slot.albumUI = this;
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
    }

    public void UpdateSelectedImage()
    {
        selectedScreenshotImage.sprite = Extension.LoadSprite(curSlot.screenshot.Data.path);
    }

    private void DeleteFromAlbum()
    {
       screenshotSlots.Remove(curSlot);
       curSlot.Delete();
    }

    public void Active()
    {
        isActive = !isActive;
        albumPanel.SetActive(isActive);

        if ( !isInit )
        {
            isInit = true;
            InitAlbumUISlots();
        }    
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
        lookedPanel.SetActive(true);
    }

    public void ButtonMarking()
    {
        curSlot.UpdateMarking();
    }

}
