using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneTrigger : MonoBehaviour
{
    public string sceneName = "Forest"; // Nom de la scène cible
    public Image blackoutScreen;       // Référence à l'écran noir
    public float transitionDuration = 2f; // Durée de la transition

    private bool isTransitioning = false;

    private void Start()
    {
        if (blackoutScreen != null)
        {
            // Configurer l'écran noir pour qu'il couvre tout l'écran et soit transparent au départ
            RectTransform rectTransform = blackoutScreen.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero; // Bas-gauche
            rectTransform.anchorMax = Vector2.one; // Haut-droite
            rectTransform.offsetMin = Vector2.zero; // Pas de décalage
            rectTransform.offsetMax = Vector2.zero; // Pas de décalage
            blackoutScreen.color = new Color(0, 0, 0, 0); // Initialement transparent
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet entrant est le joueur
        if (other.CompareTag("player") && !isTransitioning)
        {
            StartCoroutine(TransitionToNewScene(sceneName));
        }
    }

    private IEnumerator TransitionToNewScene(string sceneName)
    {
        isTransitioning = true;

        float elapsedTime = 0f;

        // Transition vers l'écran noir
        while (elapsedTime < transitionDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (transitionDuration / 2)));
            yield return null;
        }

        // Attente avant de charger la nouvelle scène (optionnel)
        yield return new WaitForSeconds(transitionDuration / 2);

        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneName);

        // Réinitialiser l'écran noir (si nécessaire après la scène)
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (transitionDuration / 2)));
            yield return null;
        }

        isTransitioning = false;
    }
}
