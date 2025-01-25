using UnityEngine;

public class SfxService : MonoBehaviour
{
    private static SfxService instance;
    public static SfxService Instance => instance;
    
    [SerializeField]
    private SfxData sfxData;
    public SfxData SfxData => sfxData;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        var musicSource = PrepareSound(sfxData.MainMusic);
        musicSource.loop = true;
        DontDestroyOnLoad(musicSource.gameObject);
        musicSource.Play();
    }

    public AudioSource PrepareSound(AudioDefinition sound)
    {

        var container = new GameObject(sound.audioClip.name);
        var audioSource = container.AddComponent<AudioSource>();
        audioSource.volume = sound.volume;
        audioSource.clip = sound.audioClip;
        return audioSource;
    }

    public void PlayOneShoot(AudioDefinition sound)
    {
        var container = new GameObject(sound.audioClip.name);
        var audioSource = container.AddComponent<AudioSource>();
        audioSource.volume = sound.volume;
        audioSource.clip = sound.audioClip;
        audioSource.Play();
        Destroy(container, sound.audioClip.length + 0.2f);
    }
}
