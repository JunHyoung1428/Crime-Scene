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
    private Quaternion initialRotation; // �ʱ� ȸ����
    private Vector3 initialPosition; // �ʱ� ��ġ��
    RaycastHit hit;
    Vector3 rayOrigin;
    Vector3 rayDirection;


    // ����� �� ���·� ����
    /*    private void ZoomObject( Transform objTransform )
        {
            interactrotation = objTransform.rotation;
            initialRotation = objTransform.rotation; // �ʱ� ȸ���� ����
            initialPosition = objTransform.position; // �ʱ� ��ġ�� ����

            // ī�޶�� ��� ������ ���� ���� ����Ͽ� ����� �÷��̾� ī�޶� �ٶ󺸵��� ��
            Vector3 cameraToObject = objTransform.position - playerCamera.position;
            objTransform.rotation = Quaternion.LookRotation(cameraToObject);

            // ����� �� ��ġ�� �̵���Ŵ
            objTransform.position = Vector3.Lerp(zoomPosition.position, objTransform.position, Time.deltaTime * 2f);

            // ������Ʈ�� ���������� Ŀ�����̰�
            Cursor.lockState = CursorLockMode.None;


            isZoomed = true; // �� ���·� ����
        }

        // ����� �� ���� ����
        private void UnzoomObject( Transform objTransform )
        {
            // ����� �ʱ� ��ġ�� �̵���Ŵ
            objTransform.position = Vector3.Lerp(initialPosition, zoomPosition.position, Time.deltaTime * 2f);
            objTransform.rotation = initialRotation; // ����� ȸ���� �ʱ� ȸ�������� ����

            // �� ��ü�� Ŀ�� ����
            Cursor.lockState = CursorLockMode.Locked;

            // ������ �ٽ� ���ʹϾ��� �������־� null ���·� ����
            interactrotation = new Quaternion();
            isZoomed = false; // �� ���� ����
        }

        // �ؿ����� �� �������̽� �����
        // ������Ʈ�� ���ʹϾ�
        Quaternion interactrotation;






        // �������̽�
        // ���콺 �����ӿ� ���� �ٲ���
        public void RotationCon()
        {
            if ( hit.transform != null )
            {
                hit.transform.Rotate(interactrotation.x, 0, 0);
                hit.transform.Rotate(0, interactrotation.y, 0);
            }
        }
        // Ŀ���� �Ⱥ��϶��� ȸ���ǰ���
        private void Update()
        {
            if ( Cursor.lockState == CursorLockMode.Locked )
                RotationCon();
        }
    */

    private void Start()
    {
        initialRotation = Quaternion.identity; // �ʱ� ȸ������ �ʱ�ȭ
    }

    // ��ȣ�ۿ� �Է� ó��
    public void OnInteract( InputValue value )
    {
        rayOrigin = playerCamera.position; // ���� �������� �÷��̾� ī�޶� ��ġ
        rayDirection = playerCamera.forward; // ���� ������ �÷��̾� ī�޶��� ���� ����


        // ����ĳ��Ʈ�� ��ȣ�ۿ� ������ ��� Ȯ��
        if ( Physics.Raycast(rayOrigin, rayDirection, out hit, 100, interactableLayer) )
        {
            IRotatable rotatable = hit.transform.gameObject.GetComponent<IRotatable>();
            IZoomable zoomable = hit.transform.gameObject.GetComponent<IZoomable>();
            IReadable readable = hit.transform.gameObject.GetComponent<IReadable>();

            if(zoomable != null)
            {
                zoomable.ZoomObject(zoomPosition);
            }
        }
    }
    public void OnCancel( InputValue value )
    {
        //UnzoomObject(hit.transform); // ����� �� ����
    }
    // �������̽�
    // ���콺Ŀ���� �������� ������Ʈ�� �ȵ��ư������ְ� ������ ���ư�����
    public void OnClick( InputValue value )
    {
        /*if ( hit.transform != null )
        {
            if ( Cursor.lockState == CursorLockMode.None )
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }*/
    }
    // �������̽�
    // ���콺�� ���϶��� �Ⱥ��϶� ��Ʈ������ 
    public void OnRotationCon( InputValue value )
    {
        /*  if ( gameObject.GetComponent<CamaraController>().enabled == false )
          {
              Vector2 input = value.Get<Vector2>();

              interactrotation.x = input.x;
              interactrotation.y = input.y;
          }*/

    }
    // �б� �������̽�
    // RŰ�� ������ �б� ui�� ����������
    public void OnRead( InputValue value )
    {
        /*        Canvas readKey = gameObject.GetComponentInChildren<Canvas>(true);
                if ( readKey.enabled == false )
                    readKey.enabled = true;
                else
                    readKey.enabled = false;
        */
    }



}
