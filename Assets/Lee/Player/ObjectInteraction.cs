using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] PlayerInput control;
    [SerializeField] Transform playerCamera; // �÷��̾� ī�޶�
    [SerializeField] LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾�
    [SerializeField] Transform zoomPosition; // �� ��ġ
    [SerializeField] Image aim;
    [SerializeField] Image background;

    private bool isZoomed = false; // �� ���� ����
    private Quaternion initialRotation; // �ʱ� ȸ����
    private Vector3 initialPosition; // �ʱ� ��ġ��
    RaycastHit hit;
    Vector3 rayOrigin;
    Vector3 rayDirection;
    private void Start()
    {
        initialRotation = Quaternion.identity; // �ʱ� ȸ������ �ʱ�ȭ
    }

    // ��ȣ�ۿ� �Է� ó��
    public void OnInteraction( InputValue value )
    {
        rayOrigin = playerCamera.position; // ���� �������� �÷��̾� ī�޶� ��ġ
        rayDirection = playerCamera.forward; // ���� ������ �÷��̾� ī�޶��� ���� ����

        // ����ĳ��Ʈ�� ��ȣ�ۿ� ������ ��� Ȯ��
        if ( Physics.Raycast(rayOrigin, rayDirection, out hit, 100, interactableLayer) )
        {
            // �ܵǾ� ���� ���� ���¶��
            if ( !isZoomed )
            {
                ZoomObject(hit.transform); // ����� ��
                PlayerIdle(); // �÷��̾� ������ ����
            }

        }
    }
    public void OnCancel( InputValue value )
    {
        UnzoomObject(hit.transform); // ����� �� ����
        ResumeMovement(); // ������ Ȱ��ȭ
    }

    // ����� �� ���·� ����
    private void ZoomObject( Transform objTransform )
    {
        initialRotation = objTransform.rotation; // �ʱ� ȸ���� ����
        initialPosition = objTransform.position; // �ʱ� ��ġ�� ����

        // ī�޶�� ��� ������ ���� ���� ����Ͽ� ����� �÷��̾� ī�޶� �ٶ󺸵��� ��
        Vector3 cameraToObject = objTransform.position - playerCamera.position;
        objTransform.rotation = Quaternion.LookRotation(cameraToObject);

        // ����� �� ��ġ�� �̵���Ŵ
        objTransform.position = Vector3.Lerp(zoomPosition.position, objTransform.position, Time.deltaTime * 2f);

        // ������Ʈ�� ���������� Ŀ�����̰�
        Cursor.lockState = CursorLockMode.None;

        // �� �����϶� ���콺�� ���� ī�޶� �����̴°� ����
        gameObject.GetComponent<PlayerCameraControl>().enabled = false;

        // �� �����϶� Aim�� �����
        aim.enabled = false;

        // ui ��׶��带 ����
        background.enabled = true;

        isZoomed = true; // �� ���·� ����
    }

    // ����� �� ���� ����
    private void UnzoomObject( Transform objTransform )
    {
        // ����� �ʱ� ��ġ�� �̵���Ŵ
        objTransform.position = Vector3.Lerp(initialPosition, zoomPosition.position, Time.deltaTime * 2f);
        objTransform.rotation = initialRotation; // ����� ȸ���� �ʱ� ȸ�������� ����

        // �� ��ü�� Ŀ�� ����
        Cursor.lockState = CursorLockMode .Locked;

        // �� ������ ���콺�� �ٽ� ī�޶� �����̰� ��
        gameObject.GetComponent<PlayerCameraControl>().enabled = true;

        // aim�� �ٽ� ����
        aim.enabled = true;

        // ��׶��� ���ֱ�
        background.enabled = false;  


        isZoomed = false; // �� ���� ����
    }

    private void PlayerIdle()
    {
        control.enabled = false;
    }
    private void ResumeMovement()
    {
        control.enabled = true;
    }
}
