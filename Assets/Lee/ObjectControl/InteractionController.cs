using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    // ������Ʈ�� ���ʹϾ�
    Quaternion m_rotation;
    private void Awake()
    {
        // ������Ʈ�� ȸ���� ���ʹϾ����� ������ ����
        m_rotation = transform.rotation;
    }

    // ���콺Ŀ���� �������� ������Ʈ�� �ȵ��ư������ְ� ������ ���ư�����
    public void OnClick( InputValue value )
    {
        
        if ( Cursor.lockState == CursorLockMode.None )
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    // ���콺�� ���϶��� �Ⱥ��϶� ��Ʈ������ 
    public void OnRotationCon( InputValue value )
    {
        Vector2 input = value.Get<Vector2>();

        m_rotation.x = input.x;
        m_rotation.y = input.y;

    }

    // ���콺 �����ӿ� ���� �ٲ���
    public void RotationCon()
    {
        transform.Rotate(m_rotation.x, 0, 0);
        transform.Rotate(0, m_rotation.y, 0);
    }

    // Ŀ���� �Ⱥ��϶��� ȸ���ǰ���
    private void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        RotationCon();
    }

    // RŰ�� ������ �б� ui�� ����������
    public void OnRead(InputValue value )
    {
        Canvas readKey = gameObject.GetComponentInChildren<Canvas>(true);
        if(readKey.enabled == false) 
        readKey.enabled = true;
        else
            readKey.enabled = false;
    }

}
