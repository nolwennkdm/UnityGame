using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Nécessaire pour changer de scène
using TMPro;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    public GameObject healthBar;
    public Canvas interactionCanvas;
    public TextMeshProUGUI interactionText;
    public Image blackoutScreen;
    public float sleepDuration = 2f;

    private bool isPlayerInRangeBed = false;
    private bool isPlayerInRangedoor = false;
    private bool isPlayerInRangeEnterdoor = false; // Nouvelle variable pour le tag "enter_door"
    private bool isSleeping = false;
    private bool isTransitioning = false;

    private GameObject currentInteractionObject;

    private void Start()
    {
        // Ajuster l'image de l'écran noir pour couvrir tout l'écran
        if (blackoutScreen != null)
        {
            RectTransform rectTransform = blackoutScreen.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero; // Bas-gauche
            rectTransform.anchorMax = Vector2.one; // Haut-droite
            rectTransform.offsetMin = Vector2.zero; // Pas de décalage
            rectTransform.offsetMax = Vector2.zero; // Pas de décalage
            blackoutScreen.color = new Color(0, 0, 0, 0); // Initialement transparent
        }

        if (interactionCanvas != null)
            interactionCanvas.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
      //  Debug.Log($"Collision détectée avec {collision.gameObject.name}, tag : {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Bed"))
        {
           // Debug.Log("Collision avec un lit détectée !");
            isPlayerInRangeBed = true;
            currentInteractionObject = collision.gameObject;

            if (interactionCanvas != null)
            {
                interactionCanvas.gameObject.SetActive(true);
                interactionText.text = "Appuyez sur E pour dormir";
            }
        }
        else if (collision.gameObject.CompareTag("door"))
        {
            //Debug.Log("Collision avec une porte détectée !");
            isPlayerInRangedoor = true;
            currentInteractionObject = collision.gameObject;

            if (interactionCanvas != null)
            {
                interactionCanvas.gameObject.SetActive(true);
                interactionText.text = "Appuyez sur E pour sortir";
            }
        }
        else if (collision.gameObject.CompareTag("enter_door")) // Détecter la porte d'entrée
        {
            //Debug.Log("Collision avec la porte d'entrée détectée !");
            isPlayerInRangeEnterdoor = true;
            currentInteractionObject = collision.gameObject;

            if (interactionCanvas != null)
            {
                interactionCanvas.gameObject.SetActive(true);
                interactionText.text = "Appuyez sur E pour entrer";
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bed") || collision.gameObject.CompareTag("door") || collision.gameObject.CompareTag("enter_door"))
        {
           // Debug.Log($"Sortie de collision avec {collision.gameObject.tag} !");
            isPlayerInRangeBed = false;
            isPlayerInRangedoor = false;
            isPlayerInRangeEnterdoor = false;
            currentInteractionObject = null;

            if (interactionCanvas != null)
                interactionCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerInRangeBed && !isSleeping)
            {
               // Debug.Log("Le joueur a appuyé sur E pour dormir.");
                StartCoroutine(Sleep());
            }
            else if (isPlayerInRangedoor && !isTransitioning)
            {
                //Debug.Log("Le joueur a appuyé sur E pour sortir.");
                StartCoroutine(TransitionToNewScene("Scenes")); // Transition vers "Scenes"
            }
            else if (isPlayerInRangeEnterdoor && !isTransitioning) // Si le joueur est proche de la porte d'entrée
            {
               // Debug.Log("Le joueur a appuyé sur E pour entrer dans la maison.");
                StartCoroutine(TransitionToNewScene("House")); // Transition vers "House"
            }
        }
    }

    private IEnumerator Sleep()
    {
        isSleeping = true;

        float elapsedTime = 0f;
        while (elapsedTime < sleepDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (sleepDuration / 2)));
            yield return null;
        }

        yield return new WaitForSeconds(sleepDuration / 2);

        if (healthBar != null)
        {
            HealthBar healthBarComponent = healthBar.GetComponent<HealthBar>();
            if (healthBarComponent != null)
            {
                healthBarComponent.SetHealthToMax();
                //Debug.Log("Barre de vie restaurée au maximum !");
            }
            else
            {
               // Debug.LogWarning("Le composant HealthBar est manquant sur l'objet healthBar !");
            }
        }

        elapsedTime = 0f;
        while (elapsedTime < sleepDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (sleepDuration / 2)));
            yield return null;
        }
        //Pourquoi deux boucles ?
        isSleeping = false;
       // Debug.Log("Le joueur a fini de dormir.");
    }

   private IEnumerator TransitionToNewScene(string sceneName)
{
    isTransitioning = true;

    // Enregistrer la scène actuelle avant de la changer
    SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
    if (spawnManager != null)
    {
        spawnManager.SetPreviousScene(SceneManager.GetActiveScene().name); // Enregistrer la scène précédente
    }

    float elapsedTime = 0f;
    while (elapsedTime < sleepDuration)
    {
        elapsedTime += Time.deltaTime;
        blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / sleepDuration));
        yield return null;
    }

    // Charger la nouvelle scène après l'écran noir
    SceneManager.LoadScene(sceneName);
}

}
