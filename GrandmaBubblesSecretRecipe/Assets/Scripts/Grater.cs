using System.Collections.Generic;
using UnityEngine;

public class Grater : MonoBehaviour
{
    private List<GrateState> availablePlayers = new List<GrateState>();
    private List<Vector3> previousLocations = new List<Vector3>();
    private void Update()
    {
        for (var i = 0; i < previousLocations.Count; i++)
        {
            var previousLocation = previousLocations[i];
            var currentLocation = 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            if (!collision.attachedRigidbody.TryGetComponent<GrateState>(out var exisitingState))
            {
                var state = collision.attachedRigidbody.gameObject.AddComponent<GrateState>();
                availablePlayers.Add(state);
            } else
            {
                availablePlayers.Add(exisitingState);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            if (collision.attachedRigidbody.TryGetComponent<GrateState>(out var exisitingState))
            {
                availablePlayers.Remove(exisitingState);
            }
        }
    }
}
