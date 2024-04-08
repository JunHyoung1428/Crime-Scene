using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    //��ȣ�ۿ� ���ο� ���� �÷��̾� ���� ����
    //Ȱ��ȭ�� UI â�� ���� �÷��̾� ���� ����
    [SerializeField] PlayerController player;
    [SerializeField] ScreenshotSystem screenshotSystem;
    [SerializeField] IInteractable interactObject;

    void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
        player = gameObject.GetComponent<PlayerController>();
        screenshotSystem = Camera.main.GetComponent<ScreenshotSystem>();
    }

    public void Interaction(IInteractable interactable)
    {
        Debug.Log("Manager Interact");
        player.isInteract = true;
     
        interactObject= interactable;
        interactObject?.Interact(player);
    }

    public void OnCancel()
    {
       if(!Manager.UI.IsPopUpLeft()) //�� ���� palyer.isInteract;
       {
            Debug.Log("Manager Cancel");
            player.isInteract = false;
            interactObject?.UnInteract(player);
       }
       Manager.UI.ClosePopUpUI();
    }

    void OnPause(InputValue inputValue){
        if ( Manager.UI.IsPopUpLeft() ) return;
        if ( inputValue.isPressed )
        {

        }
    }

    public void OnScreenshot(InputValue inputValue)
    {
        if ( inputValue.isPressed )
        {
            screenshotSystem.isTakeScreenshot=true;
        }
    }

    public void OnRead( InputValue inputValue )
    {
        if( inputValue.isPressed )
        {
            if(interactObject is IReadable )
            {
                IReadable readable = ( IReadable ) interactObject;
                readable.Read();
            }
            //ReadableObject readable = interactObject. ��ȣ�ۿ����� ����� Read UI PopUP �ϵ��� ��û
        }
    }

    public void OnAlbum( InputValue inputValue )
    {
        if ( inputValue.isPressed )
        {
            screenshotSystem.OpenAlbum();
            player.isInteract = screenshotSystem.IsOpend();
        }
    }
}
