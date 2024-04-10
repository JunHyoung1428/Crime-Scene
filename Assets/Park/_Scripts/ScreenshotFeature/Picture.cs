using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Picture : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler  ,IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] Image screenshot;
    [SerializeField] Image outLine;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Color selectedColor;
    [SerializeField] Color deleteColor;
    private Color defaultColor;

    float dragSpeed = 0.65f;
    private Transform prevTransform; 

    public void SetSprite(Image image)
    {
        screenshot.sprite = image.sprite;
    }


    /******************************************************
     *             Mouse Pointer  Interfaces
     ******************************************************/

    public void OnDrag( PointerEventData eventData )
    {
        Vector3 delta = eventData.delta;
        delta.z = 0;
        transform.position += delta*Time.deltaTime*dragSpeed;

        //raycast �ؼ� ���� ������ �������� ���� color ����
        outLine.color = selectedColor;
        // outLine.color = deleteColor;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        //if ���� ���� ���� ���� ȭ��Ʈ���� ���� ���̶��
        // transform.position = prevTransform.position;  ���� ��ġ�� �ٽ� ���ư� / Ȥ�� Destroy(gameObject) 
    }

    public void OnPointerUp( PointerEventData eventData )
    {
    }



    public void OnPointerEnter( PointerEventData eventData )
    {
        outLine.color = selectedColor;
    }

    public void OnPointerExit( PointerEventData eventData )
    {
        outLine.color = defaultColor;
    }
}
