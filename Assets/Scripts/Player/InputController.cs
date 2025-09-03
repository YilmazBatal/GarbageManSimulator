using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerInteraction playerInteraction;

    void OnMove(InputValue value)
    {
        playerController.moveInput = value.Get<Vector2>();
    }
    void OnLook(InputValue value)
    {
        playerController.lookInput = value.Get<Vector2>();
    }
    void OnSprint(InputValue value)
    {
        playerController.sprintInput = value.isPressed;
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (!UIManager.Instance.isAnyPanelOpen)
            {
                playerController.TryJump();
            }
        }
    }
    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            playerInteraction.TryInteract();
        }
    }
    void OnThrow(InputValue value)
    {
        if (value.isPressed)
        {
            playerInteraction.TryThrow();
        }
    }
    void OnEscape(InputValue value)
    {
        if (value.isPressed)
        {
            if (UIManager.Instance.activePanel == null)
                UIManager.Instance.OpenPanel(UIManager.Instance.pausePanel);
            else
                UIManager.Instance.ClosePanel();
        }
    }
    void OnSkillMenu(InputValue value)
    {
        if (value.isPressed)
        {
            if (UIManager.Instance.activePanel == null)
                UIManager.Instance.OpenPanel(UIManager.Instance.skillMenu);
            else if (UIManager.Instance.activePanel == UIManager.Instance.skillMenu)
                UIManager.Instance.ClosePanel();
            else if (UIManager.Instance.isAnyPanelOpen)
            {
                if (UIManager.Instance.isActivePanelMinigame)
                    ToastNotification.Show("Can't open skill menu in puzzles", 2, "alert");
                else
                {
                    UIManager.Instance.ClosePanel();
                    UIManager.Instance.OpenPanel(UIManager.Instance.skillMenu);
                }
            }
        }
    }
}
