using UnityEngine;

public class PossessableGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject possessablePrefab;

    [SerializeField]
    private Transform spawnLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody.TryGetComponent<BubblePossesable>(out var bubblePossesable))
        {
            return;
        }

        bubblePossesable.RegisterPossesable(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody.TryGetComponent<BubblePossesable>(out var bubblePossesable))
        {
            return;
        }

        bubblePossesable.DeregisterPossesable(this);
    }

    public IPossesable GetPossesableInstance(Transform parent)
    {
        var possessableInstance = Instantiate(possessablePrefab, spawnLocation.position, Quaternion.identity, parent).GetComponent<IPossesable>();
        return possessableInstance;
    }
}
