using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    [SerializeField] Transform playerCamera; // �÷��̾� ī�޶�
    [SerializeField] LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾�
    [SerializeField] Transform zoomPosition; // �� ��ġ

    private bool isZoomed = false; // �� ���� ����
    public bool IsZoomed { get { return isZoomed; } }
    RaycastHit hit;
    Vector3 rayOrigin;
    Vector3 rayDirection;
    IRotatable rotatable;
    IZoomable zoomable;
    IReadable readable;
    private bool isreading = false;


    // �������̽�
    // ���콺 �����ӿ� ���� �ٲ��� 
    public void RotationCon()
    {
        if ( rotatable != null )
        {
            rotatable.Rotation();
        }
        else { return; }
    }


    // Ŀ���� �Ⱥ��϶��� ȸ���ǰ���
    private void Update()
    {
        if ( Cursor.lockState == CursorLockMode.Locked )
            RotationCon();
    }

    // ��ȣ�ۿ� �Է� ó�� (���콺 ��Ŭ��) 
    public void OnInteract( InputValue value )
    {
        rayOrigin = playerCamera.position; // ���� �������� �÷��̾� ī�޶� ��ġ
        rayDirection = playerCamera.forward; // ���� ������ �÷��̾� ī�޶��� ���� ����


        // ����ĳ��Ʈ�� ��ȣ�ۿ� ������ ��� Ȯ��
        if ( Physics.Raycast(rayOrigin, rayDirection, out hit, 100, interactableLayer) )
        {
            rotatable = hit.transform.gameObject.GetComponent<IRotatable>();
            zoomable = hit.transform.gameObject.GetComponent<IZoomable>();
            readable = hit.transform.gameObject.GetComponent<IReadable>();

            if ( zoomable != null && isZoomed == false )
            {
                zoomable.ZoomObject(zoomPosition);
                isZoomed = true;
            }
        }
    }

    // QŰ 
    public void OnCancel( InputValue value )
    {
        if ( zoomable != null && isreading == false )
        {
            zoomable.UnzoomObject(zoomPosition);
            rotatable = null;
            zoomable = null;
            readable = null;
            isZoomed = false;
        }
    }
    // �������̽�
    // ���콺Ŀ���� �������� ������Ʈ�� �ȵ��ư������ְ� ������ ���ư����� 
    // ���콺 ��Ŭ��
    public void OnClick( InputValue value )
    {
        if ( hit.transform != null && readable == null )
        {
            if ( Cursor.lockState == CursorLockMode.None )
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
    // �������̽�
    // ���콺�� ���϶��� �Ⱥ��϶� ��Ʈ������ 
    public void OnRotationCon( InputValue value )
    {
        if ( rotatable != null )
            rotatable.RotationTrnas(value);

    }
    // �б� �������̽�
    // RŰ�� ������ �б� ui�� ����������
    public void OnRead( InputValue value )
    {
        if ( isreading == false )
        {
            if ( readable != null )
            {
                readable.Read();
                isreading = true;
            }
        }
        else
        {
            if ( readable != null )
            {
                readable.Read();
                isreading = false;
            }
        }

    }

}
