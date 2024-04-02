using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScreenshotTest : MonoBehaviour
{
    // �׽�Ʈ�� ��ũ��Ʈ
    // C�� ������� �ൿ ��ü�� �Ͻ����� �ܸ̿� �� �Ǿ��ؼ� GameManager�� ������ �־�� �� ��
    [SerializeField] ScreenshotSystem screenshotSystem;

    void OnCapture(InputValue inputValue )
    {
        if(inputValue.isPressed)
        {
            screenshotSystem.isTakeScreenshot = true;
        }
    }
}
