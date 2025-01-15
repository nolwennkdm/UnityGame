using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float distance = 5f; // Distance de la caméra par rapport au joueur
    public float height = 2f; // Hauteur de la caméra par rapport au joueur
    public float rotationSpeed = 5f; // Vitesse de rotation de la caméra
    public float smoothSpeed = 0.125f; // Vitesse de lissage pour un mouvement fluide

    private Vector3 currentVelocity;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Aucun joueur assigné à la caméra TPS !");
            return;
        }
    }

    void Update()
    {
        // Rotation de la caméra en fonction de la souris
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Appliquer la rotation
        transform.RotateAround(player.position, Vector3.up, horizontalInput);

        // Limiter l'angle vertical de la caméra pour éviter que la caméra ne passe sous le joueur
        float currentXRotation = transform.eulerAngles.x;
        float newXRotation = Mathf.Clamp(currentXRotation - verticalInput, -30f, 60f);
        transform.eulerAngles = new Vector3(newXRotation, transform.eulerAngles.y, transform.eulerAngles.z);

        // Calculer la position de la caméra en suivant le joueur à la bonne distance et hauteur
        Vector3 desiredPosition = player.position - transform.forward * distance + Vector3.up * height;

        // Lissage de la position de la caméra pour un mouvement plus fluide
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
    }
}
