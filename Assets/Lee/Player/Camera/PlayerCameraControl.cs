using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCamera;

    int sitDownCount=0;
    // ĳ���� ũ�⿡ ���� ���� ���¿� ���ִ� ���°� �޶� ���� ����
    Vector3 SitDown1State = new Vector3(0, 0.5f, 0);
    Vector3 SitUpState = new Vector3(0, 1, 0);
    Vector3 SitDown2State = new Vector3(0, 0.25f, 0);
    private void OnZoom( InputValue value )
    {
        Zoom();
    }
    private void Zoom()
    {
        if ( mainCamera.m_Lens.FieldOfView == 60 )
            mainCamera.m_Lens.FieldOfView = 40;
        else
            mainCamera.m_Lens.FieldOfView = 60;
    }
    private void OnSitDown( InputValue value )
    {
        CinemachineTransposer transposer = mainCamera.GetCinemachineComponent<CinemachineTransposer>();

        if ( sitDownCount == 0 )
        {
            transposer.m_FollowOffset = SitDown1State;
            sitDownCount = 1;
        }
        else if ( sitDownCount == 1 ) 
        {
            transposer.m_FollowOffset = SitDown2State;
            sitDownCount = 2;
        }
        else if(sitDownCount == 2 )
        {
            transposer.m_FollowOffset = SitUpState;
            sitDownCount = 0;
        }
    }
    // ���콺

    // ī�޶��� ���Ʒ��� ������ ���Ʒ��� ��
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;
    // �÷��̾��� �����̼��� ���� ī�޶� ���� ���ư���
    [SerializeField] Transform player;
    private Vector2 inputDir;
    private float xRotation;

    private void OnEnable()
    {
        // ���콺�� ����� ������ ���ڸ��� �ְ�����
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        // ����� ����� ������
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {

        xRotation -= inputDir.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        
        player.transform.Rotate(Vector3.up, inputDir.x * mouseSensitivity * Time.deltaTime);

        transform.Rotate(Vector3.up, mouseSensitivity * inputDir.x * Time.deltaTime);
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void OnLook( InputValue value )
    {
        inputDir = value.Get<Vector2>();
    }

}
