using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenshotSystem : MonoBehaviour
{
    // Model
    //Screenshot을 찍고 관리하는 스크립트 

    //* MainCamera에 이 컴포넌트를 부착해야 사용가능

    /***********************************************************************
    *                               Fields & Properties
    ***********************************************************************/

    [SerializeField] string folderName = "ScreenShots";
    [SerializeField] string fileName = "MyScreenShot";
    [SerializeField] string extName = "png";

    [SerializeField, Range(20, 100)] int maxCapacity; //최대 이미지 갯수
    [SerializeField] ScreenshotAlbumUI albumUI;
    [SerializeField] PopUpUI ViewFinder;
    [SerializeField] AudioClip sfx;

    public bool isTakeScreenshot;

    #region Properties


    //경로 설정
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
    private string TotalPath => $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}"; //일단 작명 규칙은 현재 시간참조

    private string lastSavedPath;

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/

    private void Start()
    {
        ScreenshotAlbum.Instance.InitAlbum(FolderPath);
        albumUI = FindAnyObjectByType<ScreenshotAlbumUI> ();
    }

    // 카메라가 랜더링 마친 이후임을 보장하기 위해 OnPostRender()에서 호출
    // .. 인데 OnPostRender는 시네머신 컴포넌트에서는 호출 안해주니 유의
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

    //스크린샷을 찍는 메소드
    #region ScreenShot
    private void TakeScreenShot()
    {
        string totalPath = TotalPath; // 프로퍼티 참조 시 시간에 따라 이름이 결정되므로 캐싱

       

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

        // 현재 스크린으로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        screenTex.ReadPixels(area, 0, 0); //OnPostRender() 혹은 코루틴의 EndOfFrame이 아니면 에러 뜸

        bool succeeded = true;
        try
        {
            if ( Directory.Exists(FolderPath) == false )
            {
                Directory.CreateDirectory(FolderPath);
            }
            // 스크린샷 저장
            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG()); //Texture2D => PNG 
        }
        catch ( Exception e ) // 예외처리
        {
            succeeded = false;
            Debug.LogWarning($"Screenshot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }
        Destroy(screenTex); //가비지 제거 

        ScriptableObject.CreateInstance<ScreenshotData>();
        //List에 사진 객체 추가
        ScreenshotAlbum.Instance.Add(new Screenshot(Extension.CreateScreenshotData(totalPath)));

        if ( succeeded )
        {
            Debug.Log($"Screenshot Saved : {totalPath}");
            StartCoroutine(ScreenshotAnimation());
            lastSavedPath = totalPath; // 최근 경로에 저장
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