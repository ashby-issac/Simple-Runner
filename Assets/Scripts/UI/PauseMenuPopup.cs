using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuPopup : PanelPopup
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI timeTakenText;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;

    private void OnResumeClicked()
    {
        UIManager.Instance.HideAnyActivePopup();
        Time.timeScale = 1;
    }

    public override void OnMainMenuClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIManager.showDirectGameplay = false;
    }

    public override void OnQuitClicked()
    {
        base.OnQuitClicked();
    }

    public override void Initialize()
    {
        distanceText.text = $"Distance: {(int)PlayerMovement.DistanceCovered}";
        timeTakenText.text = $"Time: {(int)PlayerMovement.TimeElaped}";
    }

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() => OnResumeClicked());
        mainMenuBtn.onClick.AddListener(() => OnMainMenuClicked());
        quitBtn.onClick.AddListener(() => OnQuitClicked());
    }
}
