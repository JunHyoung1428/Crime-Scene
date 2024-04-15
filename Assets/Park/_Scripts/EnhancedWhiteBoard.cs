using Cinemachine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnhancedWhiteBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IInteractable
{
    //���� �׸� �� ���� ���ο� LineRender ��ü�� �����ϴ� ����

    // ���� : ���� ������ �����ϱ� ������. ���߿� ���� ��ü�� ���忡 �߰��Ҷ� SortingLayer ������ ���� ���� �� ����
    // ���� : Save&Load �� ���ϰ� ŭ
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] LineRenderer linePrefab;
    [SerializeField] WhiteBoardUI whiteBoardUI;
    [SerializeField] Color color = Color.black;
    [SerializeField] LayerMask drawMask;
    [SerializeField] LayerMask pictureMask;

    LayerMask defaultMask;
    PhysicsRaycaster raycaster;
    [SerializeField] GameObject overUICam;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private LineRenderer curLine;

    private bool isDrawing;
    private bool isEdit;
    private Vector3 lineOffset = new Vector3(0, 0, -0.1f);
    private Vector3 playerPrevPos;

    void Awake()
    {
        raycaster = Camera.main.GetComponent<PhysicsRaycaster>();
        defaultMask = raycaster.eventMask;  
    }

    public void AddLine(LineRenderer line )
    {
        lines.Add(line);
    }

    void OnDisable()
    {
        raycaster.eventMask = defaultMask;
        //Manager.Data.SaveLines(lines);

    }

    /******************************************************
     *             Mouse Pointer  Interfaces
     ******************************************************/
    #region Interfaces

    public void OnPointerDown( PointerEventData eventData )
    {
        if ( isEdit ) return;

        isDrawing = true;

        Vector3 downPos = eventData.GetLocalPosition(transform);
        downPos = new Vector3(downPos.x, downPos.y, 0.2f);
        //curLine = Instantiate(linePrefab, transform.position+lineOffset, Quaternion.identity, transform);
        curLine = Instantiate(linePrefab, transform);
        curLine.startColor = color;
        curLine.endColor = color;
        lines.Add(curLine);

        Vector3 [] positions = new Vector3 [1];
        positions [0] = downPos;
        curLine.SetPositions(positions);
    }
    public void OnDrag( PointerEventData eventData )
    {
        if ( isEdit ) return;


        if ( isDrawing == false )
            return;

        Vector3 [] positions = new Vector3 [curLine.positionCount + 1];
        curLine.GetPositions(positions);
        Vector3 downPos = eventData.GetLocalPosition(transform);
        downPos = new Vector3(downPos.x, downPos.y, 0.2f);
        positions [curLine.positionCount] = downPos;

        curLine.positionCount++;
        curLine.SetPositions(positions);
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        isDrawing = false;
    }

    public void OnPointerExit( PointerEventData eventData )
    {
        isDrawing = false;
    }

    #endregion


    /******************************************************
     *             Interact  Interfaces
    ******************************************************/
    public void Interact( PlayerController interacter )
    {
        vCam.Priority = 100;
        playerPrevPos = interacter.transform.position;
        interacter.transform.position = vCam.transform.position;
        //Manager.UI.ShowPopUpUI(popUpUI); // �굵 enable on off �������� �ٲ���� // OverUICamera On
        Manager.UI.ShowWhiteBoardUI(whiteBoardUI);
    }

    public void UnInteract( PlayerController interacter )
    {
        Manager.UI.ClosePopUpUI();
        interacter.transform.position = playerPrevPos;
        overUICam.SetActive(false);
        vCam.Priority = 0;
        Manager.Data.SaveLines(lines);
    }


    /******************************************************
     *                         ETC
     ******************************************************/

    public void SetColor( Color color )
    {
        this.color = color;
    }

    /******************************************************
     *                  UI OnClick Events
     ******************************************************/

    public void SetColorButton( int color )
    {
        isEdit = false;
        overUICam.SetActive(isEdit);
        raycaster.eventMask = drawMask;
        //�Ű������� Enum�̸� OnClick�� ��� �Ұ�...
        Color newColor = new Color();
        // 0 = black, 1 = red, 2 = blue
        switch ( color )
        {
            case 0:
                newColor = Color.black;
                break;
            case 1:
                newColor = Color.red;
                break;
            case 2:
                newColor = Color.blue;
                break;
        }
        SetColor(newColor);
    }

    //��� ���� ����
    public void EraseAll()
    {
        for ( int i = 0; i < lines.Count; i++ )
        {
            Destroy(lines [i].gameObject);
        }
        lines.Clear();
    }

    //������ �׾��� ���� ����
    public void Undo()
    {
        if ( lines.Count <= 0 )
            return;

        Destroy(lines [lines.Count - 1].gameObject);
        lines.RemoveAt(lines.Count - 1);
    }

    public void Edit()
    {
        isEdit = true;
        overUICam.SetActive(isEdit);
        raycaster.eventMask = pictureMask;
    }

}

struct PointData
{
    public Vector3 position;
    public Color color;
}