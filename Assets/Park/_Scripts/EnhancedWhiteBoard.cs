using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnhancedWhiteBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IInteractable
{
    //선을 그릴 때 마다 새로운 LineRender 객체를 생성하는 버전

    // 장점 : 선을 개별로 관리하기 용이함. 나중에 사진 객체를 보드에 추가할때 SortingLayer 설정을 따로 해줄 수 있음
    // 단점 : Save&Load 시 부하가 큼
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] LineRenderer linePrefab;
    [SerializeField] WhiteBoardUI whiteBoardUI;
    [SerializeField] Color color = Color.black;
    [SerializeField] LayerMask drawMask;
    [SerializeField] LayerMask pictureMask;

    LayerMask defaultMask;
    PhysicsRaycaster raycaster;
    [SerializeField] GameObject overUICam;

    private Stack<LineRenderer> lines = new Stack<LineRenderer>();
    private LineRenderer curLine;

    private bool isDrawing;
    private bool isEdit;
    private Vector3 playerPrevPos;

    void Awake()
    {
        raycaster = Camera.main.GetComponent<PhysicsRaycaster>();
        defaultMask = raycaster.eventMask;
    }

    public void AddLine( LineRenderer line )
    {
        lines.Push(line);
    }

    void OnDisable()
    {
        raycaster.eventMask = defaultMask;
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
        curLine = Instantiate(linePrefab, transform);
        curLine.startColor = color;
        curLine.endColor = color;
        lines.Push(curLine);

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
        //매개변수가 Enum이면 OnClick에 등록 불가...
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

    //모든 선을 지움
    public void EraseAll()
    {
         while (lines.Count > 0)
        {
            Destroy(lines.Pop().gameObject);
        }
    }

    //이전에 그었던 선을 지움
    public void Undo()
    {
        if ( lines.Count <= 0 )
            return;

        Destroy(lines.Pop().gameObject);
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
