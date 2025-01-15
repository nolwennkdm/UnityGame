using UnityEngine;

public class Chest : MonoBehaviour
{
    public int goldReward = 500; // Argent donné par le coffre
    public Animator animator;    // Animation d'ouverture du coffre
    private bool isOpen = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player") && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            HealthBar playerStats = other.GetComponent<HealthBar>();
            if (playerStats != null)
            {
                OpenChest(playerStats);
            }
        }
    }

    void OpenChest(HealthBar playerStats)
    {
        if (playerStats != null)
        {
            playerStats.AddGold(goldReward);
            animator.SetTrigger("open");
            Debug.Log($"{goldReward}G ajouté au joueur.");
        }

        isOpen = true;
        Destroy(gameObject, 2f); // Optionnel : détruire le coffre après l'ouverture
    }
}
