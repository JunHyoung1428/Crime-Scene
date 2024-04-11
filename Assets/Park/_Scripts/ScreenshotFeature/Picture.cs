using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Picture : MonoBehaviour, IDragHandler,IEndDragHandler , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image screenshot;
    [SerializeField] Image outLine;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Color selectedColor;
    [SerializeField] Color deleteColor;
    private Color defaultColor;

    [SerializeField][Range(0,2)] float dragSpeed = 0.65f;
    private Transform prevTransform;
    private Vector3 offset = new Vector3(0, 0, -0.2f);

    public void SetSprite( Image image )
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

        if ( Physics.Raycast(transform.position, transform.forward, out RaycastHit hit) )
        {
            if(hit.collider.GetComponent<EnhancedWhiteBoard>() != null){
                outLine.color = selectedColor;
            }
            else
            {
                outLine.color = deleteColor;
            }
        }
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        //���� ���� ���� ���� ȭ��Ʈ���� ���� ���̶�� ���� 
        if(outLine.color == deleteColor)
        {
            Destroy(gameObject);
        }
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
