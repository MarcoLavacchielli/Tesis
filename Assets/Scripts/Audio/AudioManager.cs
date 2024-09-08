using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource[] musicSource, sfxSource;

    private AudioClip currentSFXClip;

    [SerializeField] private int musicaIndex;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ValorSliderMusica"))
        {
            float valorRecuperadoMusica = PlayerPrefs.GetFloat("ValorSliderMusica");
            MusicVolume(valorRecuperadoMusica);
        }

        if (PlayerPrefs.HasKey("ValorSliderSFX"))
        {
            float valorRecuperadoSFX = PlayerPrefs.GetFloat("ValorSliderSFX");
            SFXVolume(valorRecuperadoSFX);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(0);
        StopMusic(1);
    }

    public void PlayMusic(int index)
    {
        musicSource[index]?.Play();
    }

    public void StopMusic(int index)
    {
        if (musicSource[index] == null)
        {
            Debug.Log("Music Source Not Found");
        }
        else
        {
            musicSource[index].Stop();
        }
    }

    public void PlaySfx(int index)
    {
        sfxSource[index]?.Play();
    }

    public void PlayFirstMusic()
    {
        var firstMusicClip = musicSounds.Where(sound => sound.name.StartsWith("Music")).FirstOrDefault()?.clip;
        if (firstMusicClip != null && musicSounds.Any())
        {
            musicSource.FirstOrDefault()?.PlayOneShot(firstMusicClip);
        }
    }
    public void SkipTimeOfSFX(int index, float time)
    {
        sfxSource[index].time=time;
    }

    public void MusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("ValorSliderMusica", volume);
        musicSource.ToList().ForEach(item => item.volume = volume);
    }

    public void SFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("ValorSliderSFX", volume);
        sfxSource.ToList().ForEach(item => item.volume = volume);
    }

    public void PauseSFX(int index)
    {
        if (sfxSource[index] == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource[index].Pause();
        }
    }

    public void StopSFX(int index)
    {
        if (sfxSource[index] == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource[index].Stop();
            sfxSource[index].loop = false;
        }
    }

    public void PlaySFXLoop(int index)
    {


        if (sfxSource[index] == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource[index].loop = true;
            sfxSource[index].Play();
        }
    }

    public void PauseSFXLoop(int index)
    {
        if (sfxSource[index] == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource[index].Pause();
            sfxSource[index].Stop();
        }
    }

}