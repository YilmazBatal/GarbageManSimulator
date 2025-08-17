using UnityEngine;

public enum MinigameType {Smash, HitGreen}
public class MinigameInteract : MonoBehaviour, IInteractable
{
    [SerializeField] MinigameType minigameType; // Type of the minigame to be played // choosed in inspector
    [SerializeField] Minigames minigameUIPrefabs; // script that holds UI Prefabs for minigames

    GameObject minigameUI;

    void Start()
    {
        switch (minigameType)
        {
            case MinigameType.Smash:
                minigameUI = minigameUIPrefabs.smashBoxUI;
                break;
            case MinigameType.HitGreen:
                minigameUI = minigameUIPrefabs.hitGreenUI;
                break;
        }
    }


    public void Interact()
    {
        UIManager.Instance.GenerateMinigamePanel(minigameUI, gameObject);
    }



}