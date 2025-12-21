using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthText.text = "HP: " + currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthText.text = "HP: " + currentHealth;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        //TODO: proper game lost screen
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
