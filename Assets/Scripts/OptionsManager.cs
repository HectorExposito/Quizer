using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private AudioPlayer audioPlayer;
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("FxVolume"))
        {
            fxSlider.value = PlayerPrefs.GetFloat("FxVolume");
        }
        else
        {
            SetFxVolume();
        }
        audioPlayer.SetMusicVolume();
        audioPlayer.SetFXVolume();
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        audioPlayer.SetMusicVolume();
    }

    public void SetFxVolume()
    {
        PlayerPrefs.SetFloat("FxVolume", fxSlider.value);
        audioPlayer.SetFXVolume();
    }

}
