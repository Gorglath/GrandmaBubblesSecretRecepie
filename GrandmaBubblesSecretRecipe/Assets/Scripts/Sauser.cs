using Unity.VisualScripting;
using UnityEngine;

public class Sauser : MonoBehaviour
{
    [SerializeField]
    private SauserListener listener;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            foreach (var sausablePlayer in listener.SausablePlayers)
            {
                if(!sausablePlayer.TryGetComponent<Sauce>(out _))
                {
                    sausablePlayer.AddComponent<Sauce>();
                }
            }
        }
    }
}
