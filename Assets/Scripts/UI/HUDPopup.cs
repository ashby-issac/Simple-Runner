using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI coinsText;

    public override void Initialize()
    {
        if (!isInitialized)
        {
            UpdateHUD();
            isInitialized = true;
        }
    }

    private void UpdateHUD()
    {
        var playerHealth = PlayerPrefs.GetFloat("PlayerHealth");

        lifeText.text = $"Life: {playerHealth}";
        distanceText.text = $"Distance: {(int)PlayerMovement.DistanceCovered}" ;
        timeText.text = $"Time: {(int)PlayerMovement.TimeElaped}";
        coinsText.text = $"Coins: {PlayerMovement.CoinsCollected}";
    }

    private void Update()
    {
        if (isInitialized)
            UpdateHUD();
    }

    private void OnDisable()
    {
        isInitialized = false;
    }
}
