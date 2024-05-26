using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI timeTakenText;

    [SerializeField] private Button startBtn;
    [SerializeField] private Button quitBtn;

    [SerializeField] private BootLoader bootLoader;

    private PlayerData playerData;

    public void OnStartClicked()
    {
        UIManager.Instance.HideAnyActivePopup();
        bootLoader.gameObject.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public override void Initialize()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));

            distanceText.text = $"BestDist: {(int)playerData.BestDistance}";
            timeTakenText.text = $"BestTime: {(int)playerData.BestTime}";
        }
        else
        {
            distanceText.text = $"BestDist: 0";
            timeTakenText.text = $"BestTime: 0";
        }
    }

    private void Awake()
    {
        startBtn.onClick.AddListener(OnStartClicked);
        quitBtn.onClick.AddListener(OnQuitClicked);
    }
}
