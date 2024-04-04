using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using Unity.Mathematics;

public class ScreenshotSystem : MonoBehaviour
{
    // Model
    //Screenshot�� ��� �����ϴ� ��ũ��Ʈ 

    //* MainCamera�� �� ������Ʈ�� �����ؾ� ��밡��

    /***********************************************************************
    *                               Fields & Properties
    ***********************************************************************/

    [SerializeField] string folderName = "ScreenShots";
    [SerializeField] string fileName = "MyScreenShot";
    [SerializeField] string extName = "png";

    [SerializeField, Range(20, 100)] int maxCapacity; //�ִ� �̹��� ����
    [SerializeField] ScreenshotAlbumUI albumUI; // ���� MVC�� View�� ���� ������ ���� �ʾƾ���. event�� ����� View�� ó���� �ϴ� ���� ������ �� ��.

    bool isTakeScreenshot; 

    #region Properties


    //��� ����
    private string RootPath
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Application.dataPath;
#elif UNITY_ANDROID
            return $"/storage/emulated/0/DCIM/{Application.productName}/";
            //return Application.persistentDataPath;
#endif
        }
    }
    private string FolderPath => $"{RootPath}/{folderName}";
    private string TotalPath => $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}"; //�ϴ� �۸� ��Ģ�� ���� �ð�����

    private string lastSavedPath;

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/

    private void Start()
    {
        ScreenshotAlbum.Instance.InitAlbum(FolderPath);
    }

    // ī�޶� ������ ��ģ �������� �����ϱ� ���� OnPostRender()���� ȣ��
    // .. �ε� OnPostRender�� �ó׸ӽ� ������Ʈ������ ȣ�� �����ִ� ����
    private void OnPostRender()
    {
        if ( !isTakeScreenshot )
            return;
        TakeScreenShot();
        isTakeScreenshot = false;
    }

    // debug ������ �ϴ� input�޴°� �־���� ���߿� �ٸ����� ó���Ұ�
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.I)){
            albumUI.Active();
        }
       if(Input.GetKeyDown(KeyCode.C) ){
            isTakeScreenshot=true;
        }
    }

    /***********************************************************************
    *                               Methods
    ***********************************************************************/

    //��ũ������ ��� �޼ҵ�
    #region ScreenShot
    private void TakeScreenShot()
    {
        string totalPath = TotalPath; // ������Ƽ ���� �� �ð��� ���� �̸��� �����ǹǷ� ĳ��
        
        //List�� ���� ��ü �߰�
        ScreenshotAlbum.Instance.Add(new Screenshot(new ScreenshotData(totalPath)));

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height); 

        // ���� ��ũ�����κ��� ���� ������ �ȼ����� �ؽ��Ŀ� ����
        screenTex.ReadPixels(area, 0, 0); //OnPostRender() Ȥ�� �ڷ�ƾ�� EndOfFrame�� �ƴϸ� ���� ��

        bool succeeded = true;
        try
        {
            if ( Directory.Exists(FolderPath) == false )
            {
                Directory.CreateDirectory(FolderPath);
            }
            // ��ũ���� ����
            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG()); //Texture2D => PNG 
        }
        catch ( Exception e ) // ����ó��
        {
            succeeded = false;
            Debug.LogWarning($"Screenshot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }
        Destroy(screenTex); //������ ����

        if ( succeeded )
        {
            Debug.Log($"Screenshot Saved : {totalPath}");
            StartCoroutine(ScreenshotAnimation());
            lastSavedPath = totalPath; // �ֱ� ��ο� ����

            albumUI.UpdateAlbumUISlots();
        }
    }

    IEnumerator ScreenshotAnimation()
    {
        Debug.Log("�ƹ�ư ��¼�� ��ũ���� ȿ��,");
        yield return null;
    }

    #endregion


}