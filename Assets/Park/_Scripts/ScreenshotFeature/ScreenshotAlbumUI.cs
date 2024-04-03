using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotAlbumUI : MonoBehaviour
{
    // View
    //������� Album UI ������ ó���ϰ� ScreenshotSystem�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ
    [SerializeField] ScreenshotSystem screenshotSystem; //�̰͵� �������� ������ �ϸ� �ȵ�... ���߿� Event�� ó�� 
    [SerializeField] Image selectedScreenshot; //���õ� ��ũ���� �̹���
    [SerializeField] ScreenshotSlotUI ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab
    [SerializeField] GameObject albumPanel;
    [SerializeField] Transform albumGrid;
   
    bool isActive = false;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    private void Awake()
    {
        screenshotSystem = Camera.main.GetComponent<ScreenshotSystem>();
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
            rect.SetParent(albumGrid);
            rect.localScale = Vector3.one;
        }
    }

    private void UpdateAlbum()
    {
        Debug.Log("�ٹ� ������Ʈ");
        InitAlbum();
        // screenshotSystem�� list�� ��ȸ ��  
        // slot�� �߰� ���� ó���� ���� ����ȭ
    }

    public void Active()
    {
        isActive = !isActive;
        albumPanel.SetActive(isActive);

        UpdateAlbum();
    }
}
