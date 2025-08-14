using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panel check")]
    GameObject activePanel = null;
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
        if (activePanel != null)
        {
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

    
}
