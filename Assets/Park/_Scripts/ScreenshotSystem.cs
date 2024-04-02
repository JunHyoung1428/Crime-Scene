using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotSystem : MonoBehaviour
{

    //ī�޶� �޷��ִ� ������Ʈ�� �� ������Ʈ�� ���� �����ؾ� ��밡��

    [SerializeField] string folderName = "ScreenShots";
    [SerializeField] string fileName = "MyScreenShot";
    [SerializeField] string extName = "png";

    [SerializeField]
    public bool isTakeScreenshot;


    public List<string> screenshots = new List<string>();  //��ũ���� ��� ����� List ���߿� AlbumManager ������ �ű� ��


    /***********************************************************************
    *                               Fields & Properties
    ***********************************************************************/
    #region Properties
    private Texture2D _imageTexture; // imageToShow�� �ҽ� �ؽ���


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

    // ī�޶� ������ ��ģ �������� �����ϱ� ���� OnPostRender()���� ȣ��
    // .. �ε� OnPostRender�� �ó׸ӽ� ������Ʈ������ ȣ�� ������
    // ����ī�޶� �޾�����
    private void OnPostRender()
    {
        if ( !isTakeScreenshot )
            return;
        Debug.Log("Call ScreenShot");
        ScreenShot();
        isTakeScreenshot = false;
    }

    /***********************************************************************
    *                               Methods
    ***********************************************************************/

    private void ScreenShot()
    {
        string totalPath = TotalPath; // ������Ƽ ���� �� �ð��� ���� �̸��� �����ǹǷ� ĳ��
        screenshots.Add(totalPath);

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
            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }
        Destroy(screenTex); //������ ����

        if ( succeeded )
        {
            Debug.Log($"Screen Shot Saved : {totalPath}");
            StartCoroutine(ScreenshotAnimation());
            lastSavedPath = totalPath; // �ֱ� ��ο� ����
        }
    }

    IEnumerator ScreenshotAnimation()
    {
        Debug.Log("�ƹ�ư ��¼�� ��ũ���� ȿ��,");
        yield return null;
    }

}