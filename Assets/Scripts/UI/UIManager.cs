using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<PopupBase> panelPopups;
    [SerializeField] private BootLoader bootLoader;

    private Stack<PopupBase> popupStack = new Stack<PopupBase>();
    public static bool showDirectGameplay;

    public BootLoader BootLoader => bootLoader;

    public static UIManager Instance;

    public bool IsThereActivePopup => (popupStack.Count > 0 && popupStack.Any(popup => popup.UIPanelType == UIPanel.HUD)) || popupStack.Count < 1;
    public bool IsPausePanelActive => (popupStack.Count > 0 && popupStack.Any(popup => popup.UIPanelType == UIPanel.Pause));

    public void ShowPopup(UIPanel panelType, string data = "")
    {
        var activePopup = panelPopups.Find(popup => popup.UIPanelType == panelType);
        if (!activePopup) return;

        activePopup.Initialize();
        activePopup.gameObject.SetActive(true);

        popupStack.Push(activePopup);
    }

    public void HidePopup(UIPanel activePanel)
    {
        var activePopup = panelPopups.Find(popup => popup.UIPanelType == activePanel);
        activePopup.gameObject.SetActive(false);
    }

    public void HideAnyActivePopup()
    {
        if (popupStack.Count < 1) return;

        popupStack.Pop().gameObject.SetActive(false);
    }

    public void InitGameplayLogic()
    {
        HideAnyActivePopup();
        Invoke(nameof(ActivateBootLoader), 0f);
    }

    private void ActivateBootLoader()
    {
        BootLoader.gameObject.SetActive(true);
        BootLoader.InitOnGameStart();
    }

    public void SetRestartScene(bool isRestartScene)
    {
        //this.isRestartScene = isRestartScene;
    }

    public void ShowPausePanel()
    {
        if (IsThereActivePopup)
        {
            ShowPopup(UIPanel.Pause);
            Time.timeScale = 0;
        }
        else if (IsPausePanelActive)
        {
            HideAnyActivePopup();
            Time.timeScale = 1;
        }
    }

    private void Awake()
    {
        Instance = this;

        if (!showDirectGameplay)
        {
            ShowPopup(UIPanel.MainMenu);
        }
        else
        {
            InitGameplayLogic();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPausePanel();
        }
    }
}
