using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotAlbum : MonoBehaviour
{
    public static ScreenshotAlbum Instance { get; private set; }
    public List<Screenshot> Screenshots { get; private set; }
    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
            Screenshots = new List<Screenshot>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitAlbum( string folderPath )
    {
        if ( Directory.Exists(folderPath) == false )
        {
            Directory.CreateDirectory(folderPath);
            return;
        }

        string [] paths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        for ( int i = 0; i < paths.Length; i++ )
        {
            //Screenshots.Add(new Screenshot(new ScreenshotData(paths [i]))); //나중에 즐겨찾기 여부도 불러와야함 // ScreenshotData에 new 쓰지 말아야함 // 팩토리 패턴 적용
            Screenshots.Add(new Screenshot(Extension.CreateScreenshotData(paths [i])));
        }
    }

    public Action<Screenshot> OnScreenshotAdd;

    public void Add( Screenshot screenshot )
    {
        Screenshots.Add(screenshot);
        OnScreenshotAdd?.Invoke(screenshot);
    }

    public event Action<Screenshot> OnScreenshotDeleted;

    public void Delete( Screenshot screenshot )
    {
        string path = screenshot.Data.path;
        Screenshots.Remove(screenshot);
        if ( File.Exists(path) )
        {
            try
            {
                OnScreenshotDeleted?.Invoke(screenshot);
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

}
