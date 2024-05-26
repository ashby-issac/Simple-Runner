using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;
    private const string Obstacle_Tag = "Obstacle";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Obstacle_Tag)
        {
            currentHealth--;
            if (currentHealth < 1)
            {
                gameObject.SetActive(false);
                UIManager.Instance.ShowPopup();
            }
        }
    }
}