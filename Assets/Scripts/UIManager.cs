using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIPanel
{
    MainMenu,
    GameOver,
    Pause,
    HUD
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // OnRestartPressed
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // OnMainMenuPressed
    public void GoToMainMenu()
    {

    }

    public void ShowPopup()
    {
        Time.timeScale = 0f;


    }

    public void UpdateHUD()
    {

    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        
    }
}
