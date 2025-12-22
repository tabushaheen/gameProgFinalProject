using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Death")]
    public float reloadDelay = 1.2f; // match your death clip length

    private Animator anim;
    private bool isDead = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
        if (healthText != null) healthText.text = "HP: " + currentHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (healthText != null) healthText.text = "HP: " + currentHealth;

        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(ReloadAfterDelay());
    }

    private IEnumerator ReloadAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
