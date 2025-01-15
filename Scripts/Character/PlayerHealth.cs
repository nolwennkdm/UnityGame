using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI healthText;  // Référence au TextMeshPro pour afficher la santé
    public int maxHealth = 10;
    private int currentHealth;
    public int gold = 0; // Argent du joueur

    private void Start()
    {
        if (healthText == null)
        {
            Debug.LogError("Le TextMeshPro n'est pas assigné !");
            return;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateGoldUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }
    private void UpdateGoldUI()
    {
        // Met à jour l'UI de l'argent
        GoldUI.Instance.UpdateGoldText(gold);
    }
}
