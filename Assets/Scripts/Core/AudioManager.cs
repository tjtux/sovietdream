using UnityEngine;

/// <summary>
/// Gerenciador de áudio do jogo.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip tensionMusic;
    [SerializeField] private AudioClip victoryMusic;

    [Header("Efeitos Sonoros - Interação")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip consumeSound;

    [Header("Efeitos Sonoros - Jogo")]
    [SerializeField] private AudioClip partDeliveredSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip radiationWarningSound;
    [SerializeField] private AudioClip footstepSound;

    [Header("Configurações")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupAudioSources();
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    // Música

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayTensionMusic()
    {
        if (tensionMusic != null)
        {
            musicSource.clip = tensionMusic;
            musicSource.Play();
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic != null)
        {
            musicSource.clip = victoryMusic;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Efeitos Sonoros

    public void PlayPickupSound()
    {
        PlaySFX(pickupSound);
    }

    public void PlayDropSound()
    {
        PlaySFX(dropSound);
    }

    public void PlayInteractSound()
    {
        PlaySFX(interactSound);
    }

    public void PlayConsumeSound()
    {
        PlaySFX(consumeSound);
    }

    public void PlayPartDeliveredSound()
    {
        PlaySFX(partDeliveredSound);
    }

    public void PlayVictorySound()
    {
        PlaySFX(victorySound);
    }

    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }

    public void PlayRadiationWarningSound()
    {
        PlaySFX(radiationWarningSound);
    }

    public void PlayFootstepSound()
    {
        PlaySFX(footstepSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // Volume

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
}
