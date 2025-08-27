
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] public Sound[] musicSounds;
    [SerializeField] public Sound[] SFXSounds;
    [SerializeField] public Sound[] UISFXSounds;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] public AudioSource UISFXSource;

    [Header("Item Sounds")]
    [SerializeField] public SoundItem[] madeOf;
    [Header("Player Sounds")]
    [SerializeField] public AudioClip[] footstepClips;
    [SerializeField] public AudioClip jump;
    [SerializeField] public AudioClip land;
    [Header("Bin Sounds")]
    [SerializeField] public AudioClip lidClose;
    [SerializeField] public AudioClip lidOpen;
    [SerializeField] public AudioClip binRoll;
    [SerializeField] public AudioClip trashPile;
    [Header("Minigame Interact & sounds")]
    [SerializeField] public AudioClip interactSfx;
    [SerializeField] public AudioClip hitGreen;
    [SerializeField] public AudioClip hitRed;



    void Awake()
    {
        SingletonCheck();
    }

    void SingletonCheck()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(musicSounds, s => s.audioName == name);
        if (sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = System.Array.Find(SFXSounds, s => s.audioName == name);
        if (sound != null)
        {
            SFXSource.clip = sound.clip;
            SFXSource.Play();
        }
    }

    public void PlayUISFX(string name)
    {
        Sound sound = System.Array.Find(UISFXSounds, s => s.audioName == name);
        if (sound != null)
        {
            UISFXSource.clip = sound.clip;
            UISFXSource.Play();
        }
    }
}
