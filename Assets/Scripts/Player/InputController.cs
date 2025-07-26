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
            playerController.TryJump();
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
}
