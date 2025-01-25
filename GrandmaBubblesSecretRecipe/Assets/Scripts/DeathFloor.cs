using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var playerController = collision.GetComponentInParent<PlayerController>();
            playerController.Die();
        }
    }
}
