using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorlalScene : BaseScene
{
    [SerializeField] GameObject player;
    float AutoSaveGameTime = 5f;
    [SerializeField] GameObject WhiteBoard; 
    public override IEnumerator LoadingRoutine()
    {
        Manager.Game.PlayerFind();
        Manager.Data.LoadData();
        yield return null;
        player.transform.position = Manager.Data.GameData.tutorialData.playerPos;
        player.transform.rotation = Manager.Data.GameData.tutorialData.playerRot;
        Debug.Log(Manager.Data.GameData.tutorialData.playerPos);
        StartCoroutine(AutoSaveRutine());

    }
    IEnumerator AutoSaveRutine()
    {
        while ( true )
        {
            yield return new WaitForSeconds(AutoSaveGameTime);
            Debug.Log("�ڵ� ����");
            Manager.Data.GameData.tutorialData.playerPos = player.transform.position;
            Manager.Data.GameData.tutorialData.playerRot = player.transform.rotation;
            Manager.Data.SaveData();
        }
    }

}
