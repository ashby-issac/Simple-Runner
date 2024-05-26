using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private UIManager uiManager;

    private int currentHealth;
    private const string Obstacle_Tag = "Obstacle";

    private void Awake()
    {
        currentHealth = maxHealth;
        PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == Obstacle_Tag)
        {
            currentHealth--;
            PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
            if (currentHealth < 1)
            {
                gameObject.SetActive(false);
                uiManager.HideAnyActivePopup();
                uiManager.ShowPopup(UIPanel.GameOver);
            }
        }
    }
}