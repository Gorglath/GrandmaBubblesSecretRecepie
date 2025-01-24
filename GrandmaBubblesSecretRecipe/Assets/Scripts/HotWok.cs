using System.Collections.Generic;
using UnityEngine;

public class HotWok : MonoBehaviour
{
    [SerializeField]
    private float cookAmountPerSecond;

    private List<Cooked> availablePlayers = new List<Cooked>();
    private void Update()
    {
        for (var i = 0; i < availablePlayers.Count; i++)
        {
            availablePlayers[i].cookValue += cookAmountPerSecond * Time.deltaTime;
            if (availablePlayers[i].cookValue > 2.0f)
            {
                var playerController = availablePlayers[i].GetComponentInParent<PlayerController>();
                availablePlayers.RemoveAt(i);
                playerController.Die(true);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            if (!collision.attachedRigidbody.TryGetComponent<Cooked>(out var exisitingState))
            {
                var state = collision.attachedRigidbody.gameObject.AddComponent<Cooked>();
                availablePlayers.Add(state);
            }
            else
            {
                availablePlayers.Add(exisitingState);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            if (collision.attachedRigidbody.TryGetComponent<Cooked>(out var exisitingState))
            {
                if (availablePlayers.Contains(exisitingState))
                {
                    availablePlayers.Remove(exisitingState);
                }
            }
        }
    }
}
