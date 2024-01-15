using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource fxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    private AudioClip previousClip;
    [SerializeField] private bool isMainMenu;

    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip selectPlayer;
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip diceThrow;
    [SerializeField] private AudioClip questionMusic;
    [SerializeField] private AudioClip rightAnswerMusic;
    [SerializeField] private AudioClip wrongAnswerMusic;
    [SerializeField] private AudioClip baseSound;
    // Start is called before the first frame update
    void Start()
    {
        if (isMainMenu)
        {
            PlayMainMenu();
        }
        else
        {
            PlayGameMusic();
        }
        
    }
    private void PlaySound(AudioSource audioSource, AudioClip audio, bool loop)
    {
        audioSource.clip = audio;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayButtonClick()
    {
        PlaySound(fxAudioSource,buttonClick,false);
    }

    public void PlaySelectPlayer()
    {
        PlaySound(fxAudioSource, selectPlayer, false);
    }

    public void PlayMainMenu()
    {
        PlaySound(musicAudioSource, mainMenu, true);
    }

    public void PlayGameMusic()
    {
        PlaySound(musicAudioSource, gameMusic, true);
    }

    public void PlayDiceThrow()
    {
        PlaySound(fxAudioSource,diceThrow,false);
    }

    public void PlayQuestionMusic()
    {
        PlaySound(musicAudioSource, questionMusic, true);
    }

    public void PlayRightAnswerMusic()
    {
        PlaySound(musicAudioSource, rightAnswerMusic, false);
    }

    public void PlayWrongAnswerMusic()
    {
        PlaySound(musicAudioSource, wrongAnswerMusic, false);
    }

    public void PlayBaseSound()
    {
        PlaySound(fxAudioSource,baseSound,false);
    }
}
