using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;
    
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Méthode pour réduire la vie du monstre lorsqu'il prend des dégâts
    public void TakeDamage(int damage)
    {
        if (IsDead()) return;

        currentHealth -= damage;
        animator.SetTrigger("GetHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Méthode pour gérer la mort du monstre
    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f); // Délai pour laisser l'animation de mort jouer avant de détruire le monstre
    }

    // Vérifie si le monstre est mort
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
