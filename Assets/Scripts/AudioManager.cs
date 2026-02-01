using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private AudioSource _sfxSource;
    [SerializeField]
    private AudioClip[] _musicClips;
    [SerializeField]
    private AudioClip[] _audioClips;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        int index = (int)musicType - 1;
        if (index >= _musicClips.Length)
        {
            Debug.LogWarning("MusicType index out of range: " + index);
            return;
        }

        if (_musicClips[index] == null)
        {
            Debug.LogWarning("AudioClip not assigned for MusicType: " + musicType);
            return;
        }

        PlayMusic(_musicClips[index], true);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(SoundEffectType sfxType)
    {
        int index = (int)sfxType - 1;
        if (index >= _audioClips.Length)
        {
            Debug.LogWarning("SoundEffectType index out of range: " + index);
            return;
        }

        if (_audioClips[index] == null)
        {
            Debug.LogWarning("AudioClip not assigned for SoundEffectType: " + sfxType);
            return;
        }

        _sfxSource.PlayOneShot(_audioClips[index]);
    }
}
