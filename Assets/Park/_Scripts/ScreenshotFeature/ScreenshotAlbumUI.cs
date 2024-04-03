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
  
    public List<ScreenshotSlotUI> screenshotSlots;
    public int curIndex = 0;
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
            slot.screenshot = screenshotSystem.screenshots [i];
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
        slot.screenshot = screenshotSystem.screenshots [screenshotSystem.screenshots.Count-1];
        slot.index = screenshotSystem.screenshots.Count - 1;
        slot.albumUI = this;
        rect.SetParent(albumGrid);
        rect.localScale = Vector3.one;
        screenshotSlots.Add(slot);
    }

    private void DeleteFromAlbum()
    {
        screenshotSlots [curIndex].Remove();
        screenshotSlots.RemoveAt(curIndex);
        screenshotSystem.Delete(curIndex);
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
        lookedPanel.SetActive(true);
    }

    public void ButtonMarking()
    {
        screenshotSystem.screenshots [curIndex].Data.isBookmarked = !screenshotSystem.screenshots [curIndex].Data.isBookmarked;
        screenshotSlots [curIndex].UpdateMarking();
    }

}
