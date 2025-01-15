using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Rigidbody))] //Sinon les collisions sont bizarres
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private InputPlayer inputPlayer;
    private Rigidbody rb;

    private void Awake()
    {
        inputPlayer = GetComponent<InputPlayer>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("Rigidbody manquant sur le joueur !");
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            //Debug.Log("Aucun mouvement détecté.");
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            Vector3 velocity = direction * speed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
          // Debug.Log($"Déplacement dans la direction : {direction}");
        }
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction.magnitude == 0) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
       // Debug.Log($"Rotation vers la direction : {direction}");
    }
}
