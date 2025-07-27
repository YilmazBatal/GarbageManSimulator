using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject vehicleUI;
    public void Interact()
    {
        UIManager.Instance.OpenPanel(vehicleUI);
    }
}
