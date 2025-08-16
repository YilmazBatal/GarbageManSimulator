using UnityEngine;

public class MinigameInteract_CardboardBox : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject boxUI;

    public void Interact()
    {
        // Open the cardboard box UI when interacted with
        UIManager.Instance.OpenPanel(boxUI);
    }
    
}