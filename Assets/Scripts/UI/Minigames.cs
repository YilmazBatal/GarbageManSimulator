using UnityEngine;

public class Minigames : MonoBehaviour
{
    public static Minigames Instance { get; private set; }
    #region Minigame UIs
    [SerializeField] public GameObject smashBoxUI;
    [SerializeField] public GameObject hitGreenUI;
    #endregion

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // İkinci instance olursa sil
    }
}
