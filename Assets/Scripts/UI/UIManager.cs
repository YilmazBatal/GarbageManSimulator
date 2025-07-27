using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    PlayerController playerController;

    GameObject activePanel = null;
    public bool isAnyPanelOpen => activePanel != null;

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

    private void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
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
    
}
