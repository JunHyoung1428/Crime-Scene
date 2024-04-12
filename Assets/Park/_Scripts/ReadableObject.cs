using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReadableObject : InteractableObject , IReadable
{
    [SerializeField] PopUpUI popUpUI;
    [SerializeField] PopUpUI readInfoPrefab; 
    [SerializeField] Texture2D readInfo;
    [SerializeField] bool isAttachedToWall = false;

    public override void Interact( PlayerController player )
    {
        transform.rotation = player.ZoomedPos.rotation;
        if ( !isAttachedToWall ) {
            transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y+ 180f, transform.eulerAngles.z); //�ٴڿ� �����ִ� ��ü�� ��� ������ �ٶ󺸰� ȸ��
        }
        transform.position = player.ZoomedPos.position;
        Manager.UI.ShowPopUpUI(popUpUI);
        Cursor.visible = false;
    }

    public void Read()
    {
        Cursor.visible = true;
        if( readInfoPrefab != null)
        {
             Manager.UI.ShowPopUpUI(readInfoPrefab);
        }
        Manager.UI.CreatePopUpFromTexture(readInfo);
    }

    public override void UnInteract( PlayerController player )
    {
        base.UnInteract(player);
        Cursor.visible = true;
    }



}
