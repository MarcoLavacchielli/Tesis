using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ValorSliderMusica"))
        {
            float valorRecuperadoMusica = PlayerPrefs.GetFloat("ValorSliderMusica");
            musicSlider.value = valorRecuperadoMusica;
        }

        if (PlayerPrefs.HasKey("ValorSliderSFX"))
        {
            float valorRecuperadoSFX = PlayerPrefs.GetFloat("ValorSliderSFX");
            sfxSlider.value = valorRecuperadoSFX;
        }
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void PlayFirstMusic()
    {
        AudioManager.Instance.PlayFirstMusic();
    }
}
