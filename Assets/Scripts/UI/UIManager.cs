using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panel List")]
    [SerializeField] private List<GameObject> panels;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject skillMenu;

    [Header("Canvas")]
    [SerializeField] Canvas mainCanvas;
    [SerializeField] public ToastNotificationMessage toastNotificationMessage;

    [Header("HUD")]
    [SerializeField] public Image experienceFill;
    [SerializeField] public TMP_Text experienceText;
    [SerializeField] public TMP_Text lvCurrent;
    [SerializeField] public TMP_Text lvNext;
    [Space(15)]
    [SerializeField] public TMP_Text moneyText;
    [SerializeField] public TMP_Text dayText;
    [Space(15)]
    [SerializeField] public Image[] skillIcons;

    [Header("Panel check")]
    public GameObject activePanel = null;
    public bool isActivePanelMinigame;
    public bool isAnyPanelOpen => activePanel != null;

    [Header("Minigames")]
    public bool isSucessfull = false;
    private GameObject currentCrate;

    [Header("Vehicle UI")]
    public Button ReadyBTN;
    public TMP_Text ReadyText;

    [Header("Player")]
    [SerializeField] public Image staminaBar;
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

    void Start()
    {
        UpdateHUD();
    }
    
    #region Panel Management

    public void OpenPanel(GameObject panel)
    {
        if (panel != null)
        {
            ClosePanel();
            panel.SetActive(true);
            activePanel = panel;
            ShowCursor();
        }
    }
    public void ClosePanel()
    {

        if (activePanel != null) // there is an active panel
        {
            if (activeMinigamePanel != null) // if there is an active minigame panel
            {
                Destroy(activeMinigamePanel);
                isActivePanelMinigame = false;
                activeMinigamePanel = null;
                activePanel = null;
                HideCursor();
                return;
                
            }
            activePanel.SetActive(false);
            activePanel = null;
            HideCursor();
        }

    }

    void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // serbest
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // oyun ekranına sabit
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
        isActivePanelMinigame = true;
        currentCrate = crate;

        crate.GetComponent<MinigameInteract>().enabled = false; // Disable interaction with the crate while minigame is active so no 2nd player can use it
    }

    public void SuccessfulMinigame(GameObject minigamePanel)
    {
        isSucessfull = false;
        
        // eriştim
        currentCrate.GetComponent<MinigameInteract>().GenerateReward();
        if (ToastNotification.isStoped)
        {
            ToastNotification.Show("You are rewarded now!", 2, "success");
        }
        
        LeanTween.scale(minigamePanel, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(ClosePanel);

        currentCrate = null; // reset current crate to prevent glitches
    }

    #endregion

    #region HUD Updates

    private void UpdateHUD()
    {
        UpdateLevel();
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        // moneyText.text = "$" +  BaseData.Instance.junkyardData.moneyData.money.ToString("F2");
    }

    private void UpdateLevel()
    {
        // lvCurrent.text = BaseData.Instance.inventory.level.ToString();
        // lvNext.text = (BaseData.Instance.inventory.level + 1).ToString();
        // experienceText.text = BaseData.Instance.inventory.experience.ToString() + " / " + "000000"; // hard coded for now
    }

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
