using Unity.VisualScripting;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem sliceParticleSystem;

    [SerializeField]
    private int smallIngredientParticleAmount;

    [SerializeField]
    private int mediumIngredientParticleAmount;

    [SerializeField]
    private int bigIngredientParticleAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var playerController = collision.GetComponentInParent<PlayerController>();
            collision.attachedRigidbody.AddComponent<Sliced>();
            var sfxCollection = SfxService.Instance.SfxData.KictchenUtils.Slicer.Slice;
            var sfx = sfxCollection[Random.Range(0, sfxCollection.Length)];
            SfxService.Instance.PlayOneShoot(sfx);
            sliceParticleSystem.Emit(GetParticleCountByPlayer(collision.attachedRigidbody));
            playerController.Die(true);
        }
    }
    private int GetParticleCountByPlayer(Rigidbody2D player)
    {
        var possesable = player.GetComponent<IPossesable>();
        var playerIngredientType = possesable.IngerdientType;
        var particleAmount = 0;
        var main = sliceParticleSystem.main;
        switch (playerIngredientType)
        {
            case IngredientType.None:
            case IngredientType.Egg:
                main.startColor = Color.blue;
                particleAmount = smallIngredientParticleAmount;
                break;
            case IngredientType.Sludge:
                main.startColor = Color.magenta;
                particleAmount = smallIngredientParticleAmount;
                break;
            case IngredientType.Cabbage:
                main.startColor = Color.green;
                particleAmount = mediumIngredientParticleAmount;
                break;
            case IngredientType.Jelly:
                main.startColor = Color.red;
                particleAmount = mediumIngredientParticleAmount;
                break;
            case IngredientType.Cheese:
                main.startColor = Color.yellow;
                particleAmount = mediumIngredientParticleAmount;
                break;
            case IngredientType.Tentacle:
                main.startColor = Color.green;
                particleAmount = bigIngredientParticleAmount;
                break;
            case IngredientType.Chicken:
                main.startColor = Color.magenta;
                particleAmount = bigIngredientParticleAmount;
                break;
            case IngredientType.Flour:
                main.startColor = Color.red;
                particleAmount = bigIngredientParticleAmount;
                break;
            default:
                return bigIngredientParticleAmount;
        }

        return particleAmount;
    }
}
