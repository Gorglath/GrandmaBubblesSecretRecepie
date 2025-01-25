using System.Collections.Generic;
using UnityEngine;

public class Grater : MonoBehaviour
{
    [SerializeField]
    private GrateView grateView;

    [SerializeField]
    private float grateAmountPerMove;

    private List<GrateState> availablePlayers = new List<GrateState>();
    private List<Vector3> previousLocations = new List<Vector3>();
    private List<AudioSource> gratingAudioSources = new List<AudioSource>();
    private void Update()
    {
        for (var i = 0; i < previousLocations.Count; i++)
        {
            var previousLocation = previousLocations[i];
            if (availablePlayers[i] == null)
            {
                availablePlayers.RemoveAt(i);
                previousLocations.RemoveAt(i);
                Destroy(gratingAudioSources[i].gameObject);
                gratingAudioSources.RemoveAt(i);
                i--;
                continue;
            }
            var currentLocation = availablePlayers[i].transform.position;
            var moved = previousLocation != currentLocation;
            if (moved)
            {
                if (!gratingAudioSources[i].isPlaying)
                {
                    gratingAudioSources[i].Play();
                }

                availablePlayers[i].grateValue += grateAmountPerMove * Time.deltaTime;
                if (availablePlayers[i].grateValue > 1.0f)
                {
                    var playerController = availablePlayers[i].GetComponentInParent<PlayerController>();
                    previousLocations.RemoveAt(i);
                    availablePlayers.RemoveAt(i);
                    Destroy(gratingAudioSources[i].gameObject);
                    gratingAudioSources.RemoveAt(i);
                    playerController.Die(true);
                    return;
                }
            }
            else
            {
                if (gratingAudioSources[i].isPlaying)
                {
                    gratingAudioSources[i].Stop();
                }
            }
            previousLocations[i] = currentLocation;
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
                previousLocations.Add(state.transform.position);
                var grateDisplay = Instantiate(grateView, collision.attachedRigidbody.transform.parent);
                grateDisplay.Bind(state);

                var sfxCollection = SfxService.Instance.SfxData.KictchenUtils.Grater.Grating;
                var sfx = sfxCollection[Random.Range(0, sfxCollection.Length)];
                var audioSource = SfxService.Instance.PrepareSound(sfx);
                audioSource.loop = true;
                gratingAudioSources.Add(audioSource);
            } else
            {
                availablePlayers.Add(exisitingState);
                previousLocations.Add(exisitingState.transform.position);
                var sfxCollection = SfxService.Instance.SfxData.KictchenUtils.Grater.Grating;
                var sfx = sfxCollection[Random.Range(0, sfxCollection.Length)];
                var audioSource = SfxService.Instance.PrepareSound(sfx);
                audioSource.loop = true;
                gratingAudioSources.Add(audioSource);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            if (collision.attachedRigidbody.TryGetComponent<GrateState>(out var exisitingState))
            {
                if (availablePlayers.Contains(exisitingState))
                {
                    var locationIndex = availablePlayers.IndexOf(exisitingState);
                    availablePlayers.Remove(exisitingState);
                    previousLocations.RemoveAt(locationIndex);
                    Destroy(gratingAudioSources[locationIndex].gameObject);
                    gratingAudioSources.RemoveAt(locationIndex);
                }
            }
        }
    }
}
