using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopup : PanelPopup
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI timeTakenText;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;

    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIManager.showDirectGameplay = true;
    }

    public override void OnMainMenuClicked()
    {
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
        restartBtn.onClick.AddListener(() => OnRestartClicked());
        mainMenuBtn.onClick.AddListener(() => OnMainMenuClicked());
        quitBtn.onClick.AddListener(() => OnQuitClicked());
    }
}
