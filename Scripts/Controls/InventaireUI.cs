using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using TMPro;  

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;  // Panneau d'inventaire

    public GameObject slotPrefab;  // Préfabriqué pour un slot (button)
    public Inventory inventory;  // Référence à l'inventaire
    public GameObject player;  // Référence au Player (ajouté)



    private bool isInventoryOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false); // Cache l'inventaire au début
  
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!"); // Message d'erreur si player n'est pas assigné
            return;
        }
        //ajouter ue icone inventaire et espace pour l'ouvrir
        if (Input.GetKeyDown(KeyCode.Space))  // Ouvrir/fermer l'inventaire avec "Espace"
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        // Supprimer les anciens slots dans l'inventaire UI
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Ajouter de nouveaux slots dans l'UI pour chaque objet
        for (int i = 0; i < 8; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
            if (i < inventory.GetItems().Count)
            {
                Item item = inventory.GetItems()[i];
                TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
                Image slotImage = slot.GetComponentInChildren<Image>();

                if (slotText != null && slotImage != null)
                {
                    slotText.text = item.itemName;
                    slotImage.sprite = item.itemIcon;

                    Button button = slot.GetComponent<Button>();
                    button.onClick.AddListener(() => OnItemClick(item));  // Appelle OnItemClick avec l'item
                }
            }
            else
            {
                TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
                Image slotImage = slot.GetComponentInChildren<Image>();

                if (slotText != null && slotImage != null)
                {
                    slotText.text = "";
                    slotImage.sprite = null;

                    Button button = slot.GetComponent<Button>();
                    button.interactable = false;
                }
            }
        }
    }

    // Méthode appelée lorsque l'utilisateur clique sur un item
    private void OnItemClick(Item item)
    {
        Debug.Log($"Item clicked: {item.itemName}");
        if (item.itemName == "Patate")
    {
        HealthBar playerHealth = player.GetComponent<HealthBar>();
        if (playerHealth != null)
        {
            playerHealth.Heal(2); // Ajoute 2 PV au joueur
            Debug.Log("2 PV restaurés grâce à la patate !");
        }

        inventory.GetItems().Remove(item); // Retire la patate de l'inventaire
        UpdateInventoryUI(); // Rafraîchit l'inventaire dans l'UI
    }
    }
}
