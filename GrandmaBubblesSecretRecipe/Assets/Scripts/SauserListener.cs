using System.Collections.Generic;
using UnityEngine;

public class SauserListener : MonoBehaviour
{

    private List<PlayerController> sausablePlayers = new List<PlayerController>();
    public List<PlayerController> SausablePlayers => sausablePlayers;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var playerController = collision.GetComponentInParent<PlayerController>();
            sausablePlayers.Add(playerController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var playerController = collision.GetComponentInParent<PlayerController>();
            sausablePlayers.Remove(playerController);
        }
    }
}
