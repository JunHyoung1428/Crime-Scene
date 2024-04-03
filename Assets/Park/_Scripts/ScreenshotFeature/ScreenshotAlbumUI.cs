using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenshotAlbumUI : MonoBehaviour
{
    // View
    //������� Album UI ������ ó���ϰ� ScreenshotSystem�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ
    [SerializeField] ScreenshotSystem screenshotSystem; //�̰͵� �������� ������ �ϸ� �ȵ�... ���߿� Event�� ó�� 
    [SerializeField] public Image selectedScreenshotImage; //���õ� ��ũ���� �̹���
    [SerializeField] ScreenshotSlotUI ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab
    List<ScreenshotSlotUI> screenshotSlots;
    public int curSlotIndex = 0;
    [SerializeField] GameObject albumPanel;
    [SerializeField] Transform albumGrid;
   
    
    
    bool isActive = false;
    bool isInit = false;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    private void Awake()
    {
        screenshotSystem = Camera.main.GetComponent<ScreenshotSystem>();
        screenshotSlots = new List<ScreenshotSlotUI>();
    }


  
    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    //�׸��� ���� ScreenshotSlotUI�� �������� ����
    private void InitAlbum()
    {
        Debug.Log("�ٹ� �ʱ�ȭ");
        for ( int i = 0; i < screenshotSystem.screenshots.Count; i++ )
        {
            ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
            RectTransform rect = slot.GetComponent<RectTransform>();
            slot.path = screenshotSystem.screenshots [i].Data.path;
            slot.index = i;
            slot.albumUI = this;    
            rect.SetParent(albumGrid);
            rect.localScale = Vector3.one;
            screenshotSlots.Add(slot);
        }
    }

    public void UpdateAlbum()
    {
        Debug.Log("�ٹ� ������Ʈ");
        ScreenshotSlotUI slot = Instantiate(ScreenshotSlotUIPrefab);
        RectTransform rect = slot.GetComponent<RectTransform>();
        slot.path = screenshotSystem.screenshots [screenshotSystem.screenshots.Count-1].Data.path;
        slot.index = screenshotSystem.screenshots.Count - 1;
        slot.albumUI = this;
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
    }

    private void DeleteFromAlbum()
    {
        screenshotSlots [curSlotIndex].Remove();
        screenshotSlots.RemoveAt(curSlotIndex);
        screenshotSystem.Delete(curSlotIndex);
    }

    public void Active()
    {
        isActive = !isActive;
        albumPanel.SetActive(isActive);

        if ( !isInit )
        {
            isInit = true;
            InitAlbum();
        }
            
    }

    /***********************************************************************
    *                              OnClick Events
    ***********************************************************************/
    
    public void ButtonDelete()
    {
        DeleteFromAlbum();
    }

    public void ButtonLook()
    {
        //���� ���õ��ִ� ������ Ȯ��
    }

    public void ButtonMarking()
    {
        screenshotSystem.screenshots [curSlotIndex].Data.isBookmarked = !screenshotSystem.screenshots [curSlotIndex].Data.isBookmarked;
    }

}
