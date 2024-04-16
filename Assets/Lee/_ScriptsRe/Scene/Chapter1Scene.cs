using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Scene : BaseScene
{
    [SerializeField] GameObject player;
    float AutoSaveGameTime = 300;
    [SerializeField] EnhancedWhiteBoard WhiteBoard;
    [SerializeField] LineRenderer linePrefab;
    private void Start()
    {
        Manager.Game.InitGameManager();
    }
    public override IEnumerator LoadingRoutine()
    {
        Manager.Game.InitGameManager();
        Manager.Data.LoadData();
        yield return null;
        player = GameObject.FindGameObjectWithTag("Player");
        WhiteBoard = FindAnyObjectByType<EnhancedWhiteBoard>();
        Manager.Data.LoadLines(WhiteBoard);
        player.transform.position = Manager.Data.GameData.chapter1Data.playerPos;
        player.transform.rotation = Manager.Data.GameData.chapter1Data.playerRot;
        StartCoroutine(AutoSaveRutine());
    }
    IEnumerator AutoSaveRutine()
    {
        while ( true )
        {
            yield return new WaitForSeconds(AutoSaveGameTime);
            Debug.Log("�ڵ� ����");
            Manager.Data.GameData.chapter1Data.playerPos = player.transform.position;
            Manager.Data.GameData.chapter1Data.playerRot = player.transform.rotation;
            Manager.Data.SaveData();
        }
    }
}
