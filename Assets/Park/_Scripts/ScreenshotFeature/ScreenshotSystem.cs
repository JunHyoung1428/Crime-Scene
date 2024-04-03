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
    //Screenshot을 찍고 관리하는 스크립트 

    //* MainCamera에 이 컴포넌트를 부착해야 사용가능

    /***********************************************************************
    *                               Fields & Properties
    ***********************************************************************/

    [SerializeField] string folderName = "ScreenShots";
    [SerializeField] string fileName = "MyScreenShot";
    [SerializeField] string extName = "png";

    [SerializeField, Range(20, 100)] int maxCapacity; //최대 이미지 갯수
    [SerializeField] ScreenshotAlbumUI albumUI; // 원래 MVC는 View에 대한 참조는 하지 않아야함. event를 사용해 View에 처리를 하는 법을 생각해 볼 것.
    [SerializeField]
    public List<Screenshot> screenshots;

    bool isTakeScreenshot; 

    #region Properties


    //경로 설정
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
    private string TotalPath => $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}"; //일단 작명 규칙은 현재 시간참조

    private string lastSavedPath;

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/

    private void Awake()
    {
        screenshots = new List<Screenshot>(maxCapacity); // 나중에 저장, 불러오기시 여기서 저장데이터 참조
        LoadScreenshots();
    }

    // 카메라가 랜더링 마친 이후임을 보장하기 위해 OnPostRender()에서 호출
    // .. 인데 OnPostRender는 시네머신 컴포넌트에서는 호출 안해주니 유의
    private void OnPostRender()
    {
        if ( !isTakeScreenshot )
            return;
        Debug.Log("Call ScreenShot");
        ScreenShot();
        isTakeScreenshot = false;
    }

    // debug 용으로 일단 input받는거 넣어뒀음 나중에 다른데서 처리할것
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.I)){
            albumUI.Active();
        }
       if(Input.GetKeyDown(KeyCode.C) ){
            isTakeScreenshot=true;
        }

       //List 디버그용
       if(Input.GetKeyDown(KeyCode.D) )
        {
            for(int i=0; i < screenshots.Count; i++ )
            {
                Debug.Log(screenshots [i].Data.path);
            }
        }
    }

    /***********************************************************************
    *                               Methods
    ***********************************************************************/

    //스크린샷을 찍는 메소드
    #region ScreenShot
    private void ScreenShot()
    {
        string totalPath = TotalPath; // 프로퍼티 참조 시 시간에 따라 이름이 결정되므로 캐싱
        
        //List에 사진 객체 추가
        screenshots.Add(new Screenshot(new ScreenshotData(totalPath))); // ;; 이게 맞노

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

        if ( succeeded )
        {
            Debug.Log($"Screenshot Saved : {totalPath}");
            StartCoroutine(ScreenshotAnimation());
            lastSavedPath = totalPath; // 최근 경로에 저장

            albumUI.UpdateAlbum();
        }
    }

    IEnumerator ScreenshotAnimation()
    {
        Debug.Log("아무튼 개쩌는 스크린샷 효과,");
        yield return null;
    }


    private void LoadScreenshots()
    {
        string [] paths = Directory.GetFiles(FolderPath, "*.png", SearchOption.AllDirectories);
        for(int i=0; i<paths.Length; i++)
        {
            screenshots.Add(new Screenshot(new ScreenshotData(paths [i]))); //나중에 즐겨찾기 여부도 불러와야함
        }
        //나중에 예외처리
    }


    public void Delete(int index)
    {
        string path = screenshots [index].Data.path;
        screenshots.RemoveAt(index);
        if ( File.Exists(path) )
        {
            try
            {
                // 파일 삭제
                File.Delete(path);
                Debug.Log("File deleted: " + path);
            }
            catch ( IOException ex )
            {
                Debug.LogError("Error deleting file: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("File does not exist: " + path);
        }
    }
    #endregion


}