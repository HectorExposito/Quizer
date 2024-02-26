using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private TMP_Dropdown screenSizeDropdown;
    [SerializeField] private AudioPlayer audioPlayer;
    private Vector2 screenSize;
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
        //SetScreenSizeDropdown();
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

    private void SetScreenSizeDropdown()
    {
        if (PlayerPrefs.HasKey("ScreenSizeDropdownValue"))
        {
            screenSizeDropdown.value = PlayerPrefs.GetInt("ScreenSizeDropdownValue");
        }
        
    }
    public void ChangeScreenSize()
    {
        string size=screenSizeDropdown.options[screenSizeDropdown.value].text;
        string[] screenSizeStrings = size.Split('x');
        screenSize = new Vector2(int.Parse(screenSizeStrings[0]), int.Parse(screenSizeStrings[1]));
        PlayerPrefs.SetInt("ScreenSizeDropdownValue", screenSizeDropdown.value);
        Screen.SetResolution(((int)screenSize[0]),((int)screenSize[1]),FullScreenMode.Windowed);
    }
}
