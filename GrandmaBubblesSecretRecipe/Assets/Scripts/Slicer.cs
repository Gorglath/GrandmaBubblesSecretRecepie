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
            var sfxCollection = SfxService.Instance.SfxData.KictchenUtils.Slicer.Slice;
            var sfx = sfxCollection[Random.Range(0, sfxCollection.Length)];
            SfxService.Instance.PlayOneShoot(sfx);
            playerController.Die(true);
        }
    }
}
