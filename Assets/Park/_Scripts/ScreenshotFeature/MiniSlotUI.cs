using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] public Screenshot screenshot;

    [SerializeField] Picture prefab;

    private void Start()
    {
        image.sprite = Extension.LoadSprite(screenshot.Data.path);
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        //Ŭ���� ȭ��Ʈ ���忡 ���� �� �ִ� prefab ����
        Picture picture = Instantiate(prefab, eventData.position, transform.rotation); //���� ��ǥ �ٽ� ����������
        picture.SetSprite(image);
    }
}
