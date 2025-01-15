using UnityEngine;
using TMPro; // Pour TextMeshPro

public class GoldUI : MonoBehaviour
{
    public static GoldUI Instance;
    public TMP_Text goldText; // Lien avec le texte dans l'UI

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateGoldText(int amount)
    {
        goldText.text = $"Gold: {amount}G";
    }
}
