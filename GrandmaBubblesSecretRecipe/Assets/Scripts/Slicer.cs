using Unity.VisualScripting;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var playerController = collision.GetComponentInParent<PlayerController>();
            collision.attachedRigidbody.AddComponent<Sliced>();
            playerController.Die(true);
        }
    }
}
