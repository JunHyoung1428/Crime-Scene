using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotAlbumUI : MonoBehaviour
{
    //������� Album UI ������ ó���ϰ� ScreenshotSystem�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ
    [SerializeField] ScreenshotSystem screenshotSystem;
    [SerializeField] public GameObject albumPanel;
    public bool isActive = false;
    [SerializeField] GameObject ScreenshotSlotUIPrefab; //��ũ���� ���� UI Prefab


    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    private void Awake()
    {
        screenshotSystem = Camera.main.GetComponent<ScreenshotSystem>();
    }

    private void OnEnable()
    {

        InitAlbum();
        //TestLoadSprite();
    }

    /***********************************************************************
    *                              Methods
    ***********************************************************************/

    private void InitAlbum()
    {
        Debug.Log("�ٹ� �ʱ�ȭ");
        for ( int i = 0; i < screenshotSystem.screenshots.Count; i++ )
        {
            
            // 1. ��ο��� PNG �ҷ�����, 
            // 2. PNG�� UI�� �� �� �ִ� datatype���� (Sprite) ��ȯ
             // Extension.LoadSprite(screenshots [i]);

            //2.
            //3. �˸��� ��ġ�� ��ȯ�� �̹����� ����? ��ġ? Prefab���� ?

            // GameObject albumImage = Instantiate(screenshots[i] , transform.parent.position, true);
        }
    }

    private void TestLoadSprite()
    {
       
    }

}
