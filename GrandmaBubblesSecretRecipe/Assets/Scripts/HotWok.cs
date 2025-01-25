using System.Collections.Generic;
using UnityEngine;

public class HotWok : MonoBehaviour
{
    [SerializeField]
    private CookedView cookDisplayBar;

    [SerializeField]
    private float cookAmountPerSecond;

    private List<Cooked> availablePlayers = new List<Cooked>();
    private List<AudioSource> playerSources = new List<AudioSource>();
    private void Update()
    {
        for (var i = 0; i < availablePlayers.Count; i++)
        {
            availablePlayers[i].cookValue += cookAmountPerSecond * Time.deltaTime;
            if (availablePlayers[i].cookValue > 2.0f)
            {
                var playerController = availablePlayers[i].GetComponentInParent<PlayerController>();
                availablePlayers.RemoveAt(i);
                Destroy(playerSources[i].gameObject);
                playerSources.RemoveAt(i);
                playerController.Die(true);
                var sfx = SfxService.Instance.SfxData.KictchenUtils.HotPot.Burned;
                SfxService.Instance.PlayOneShoot(sfx);
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
                var cookedDisplay = Instantiate(cookDisplayBar, collision.attachedRigidbody.transform.parent);
                cookedDisplay.Bind(state);

                var sfx = SfxService.Instance.SfxData.KictchenUtils.HotPot.Cooking;
                var audioSource = SfxService.Instance.PrepareSound(sfx);
                audioSource.loop = true;
                playerSources.Add(audioSource);
            }
            else
            {
                var sfx = SfxService.Instance.SfxData.KictchenUtils.HotPot.Cooking;
                var audioSource = SfxService.Instance.PrepareSound(sfx);
                audioSource.loop = true;
                playerSources.Add(audioSource);
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
                    var playerIndex = availablePlayers.IndexOf(exisitingState);
                    availablePlayers.Remove(exisitingState);
                    Destroy(playerSources[playerIndex].gameObject);
                    playerSources.RemoveAt(playerIndex);
                }
            }
        }
    }
}
