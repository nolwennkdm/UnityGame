using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public Transform houseSpawnPoint; // Point de spawn devant la maison
    public Transform forestSpawnPoint; // Point de spawn près de l'allée menant à la forêt

    private void Start()
    {
        // Appeler la méthode pour placer le joueur au bon endroit selon la scène active
        SpawnPlayerAtCorrectLocation();
    }

    private void SpawnPlayerAtCorrectLocation()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Si la scène est "Scenes", vérifier d'où vient le joueur
        if (currentScene == "Scenes")
        {
            string previousScene = PlayerPrefs.GetString("PreviousScene", ""); // Récupérer la scène précédente depuis PlayerPrefs

            if (previousScene == "House")
            {
                // Le joueur vient de la maison, spawn devant la maison
                transform.position = houseSpawnPoint.position;
            }
            else if (previousScene == "Forest")
            {
                // Le joueur vient de la forêt, spawn près de l'allée menant à la forêt
                transform.position = forestSpawnPoint.position;
            }
        }
    }

    // Méthode pour enregistrer la scène précédente avant la transition
    public void SetPreviousScene(string sceneName)
    {
        PlayerPrefs.SetString("PreviousScene", sceneName);
        //a utiuliser lors des transitions 
    }
}
