using System;
using System.Collections;
using System.IO;
using UnityEngine;

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
    [SerializeField] ScreenshotAlbumUI albumUI;
    [SerializeField] MiniAlbumUI miniAlbumUI;
    [SerializeField] PopUpUI ViewFinder;
    [SerializeField] AudioClip sfx;

    public bool isTakeScreenshot;

    #region Properties


    //��� ����
    private string RootPath
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath;
#elif UNITY_ANDROID
            return $"/storage/emulated/0/DCIM/{Application.productName}/";
#else
    string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    return $"{Application.persistentDataPath}/{sceneName}";
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
        albumUI = FindAnyObjectByType<ScreenshotAlbumUI>();
        miniAlbumUI = FindAnyObjectByType<MiniAlbumUI>();
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

    /***********************************************************************
    *                               Methods
    ***********************************************************************/

    //��ũ������ ��� �޼ҵ�
    #region ScreenShot
    private void TakeScreenShot()
    {
        string totalPath = TotalPath; // ������Ƽ ���� �� �ð��� ���� �̸��� �����ǹǷ� ĳ��

        ScriptableObject.CreateInstance<ScreenshotData>();
        //List�� ���� ��ü �߰�
        ScreenshotAlbum.Instance.Add(new Screenshot(Extension.CreateScreenshotData(totalPath)));

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

            if ( albumUI.IsInit() )
            {
                albumUI.UpdateAlbumUISlots();
                miniAlbumUI.UpdateAlbumUISlots();
            }
            else
            {
                albumUI.InitAlbumUISlots();
                miniAlbumUI.InitAlbumUISlots();
            }
        }
    }

    IEnumerator ScreenshotAnimation()
    {
        Manager.UI.ShowPopUpUI(ViewFinder);
        Manager.Sound.PlaySFX(sfx);
        yield return new WaitForSeconds(0.3f);
        Manager.UI.ClosePopUpUI();
    }

    public void OpenAlbum()
    {
        if ( albumUI.IsActive() ) return;
        Manager.UI.ShowAlbumUI(albumUI);
    }

    public bool IsOpend()
    {
        return albumUI.IsActive();
    }

    #endregion


}