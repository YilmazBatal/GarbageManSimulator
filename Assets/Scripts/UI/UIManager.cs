using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas")]
    [SerializeField] Canvas mainCanvas;
    [SerializeField] public ToastNotificationMessage toastNotificationMessage;

    [Header("Panels")]
    [SerializeField] public GameObject pausePanel;

    [Header("Panel check")]
    public GameObject activePanel = null;
    public bool isAnyPanelOpen => activePanel != null;

    [Header("Vehicle UI")]
    public Button ReadyBTN;
    public TMP_Text ReadyText;

    public bool isPlayerReady = false;
    public int playerCount = 1; // It's hard coded 'till Multiplayer is implemented

    GameObject lastSelectedButton = null;

    void Awake()
    {
        SingletonCheck();
    }

    void SingletonCheck()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Panel Management

    public void OpenPanel(GameObject panel)
    {
        if (panel != null)
        {
            ClosePanel();
            panel.SetActive(true);
            activePanel = panel;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void ClosePanel()
    {

        if (activePanel != null) // there is an active panel
        {
            if (activeMinigamePanel != null) // if there is an active minigame panel
            {
                Destroy(activeMinigamePanel);
                activeMinigamePanel = null;
                activePanel = null;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
            activePanel.SetActive(false);
            activePanel = null;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
    #endregion

    #region Buttons
    // When clicked places in the map
    public void SetLastSelectedButton(GameObject button)
    {
        lastSelectedButton = button; // selected city
        ReadyBTN.enabled = true;
        isPlayerReady = true;
        int readyCount = isPlayerReady ? 1 : 0;
        ReadyText.text = $"Ready ({readyCount}/{playerCount})";

        Debug.Log("Last selected button set to: " + lastSelectedButton);
    }
    public void ReadyButton()
    {
        if (lastSelectedButton != null)
        {

            SceneManager.LoadScene(lastSelectedButton.name);
        }
    }
    #endregion

    #region Minigame Management
    GameObject activeMinigamePanel = null;
    public void GenerateMinigamePanel(GameObject minigameUI, GameObject crate)
    {
        // access canvas and attack prefab as child 
        GameObject minigamePanel = Instantiate(minigameUI, mainCanvas.transform);
        activeMinigamePanel = minigamePanel;
        activePanel = minigamePanel;

        crate.GetComponent<MinigameInteract>().enabled = false; // Disable interaction with the crate while minigame is active so no 2nd player can use it

    }
    #endregion

    #region  Pause Selection Menu

    #endregion
    
    public void ShakeUI(GameObject uiObject, float strength = 20f, float duration = 0.1f, int vibrato = 100)
    {
        Vector3 originalPos = uiObject.transform.localPosition;
        float elapsed = 0f;

        // Coroutine ile rastgele kısa hareketler
        StartCoroutine(ShakeRoutine());

        System.Collections.IEnumerator ShakeRoutine()
        {
            while (elapsed < duration)
            {
                if (uiObject == null) yield break;
                uiObject.transform.localPosition = originalPos + (Vector3)(Random.insideUnitCircle * strength);
                elapsed += Time.deltaTime;
                yield return null; // bir frame bekle
            }
            uiObject.transform.localPosition = originalPos;
        }
    }


}
