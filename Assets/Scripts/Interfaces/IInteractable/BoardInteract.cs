using UnityEngine;

public class Board : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject BoardUI;
    public void Interact()
    {
        UIManager.Instance.OpenPanel(BoardUI);
    }
}
