using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSource;
    public static SoundManager Instance;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    public void PlayBGMusicOne()
    {
        audioSource[0].Play();
    }
    public void PlayerBGMusicTwo()
    {
        audioSource[1].Play();
    }
    public void PlayEndSound()
    {
        if(audioSource[1].isPlaying)
        {
            StartCoroutine(LowerVolume(audioSource[1]));
            StartCoroutine(IncreaseVolume(audioSource[2]));
        }
        else
        {
            StartCoroutine(IncreaseVolume(audioSource[2]));
        }
    }
    public void PlayButtonPressedSound()
    {
        audioSource[3].Play();
    }
    IEnumerator LowerVolume( AudioSource audio)
    {
        while(audio.volume > 0.02)
        {
            audio.volume = Mathf.Lerp(audio.volume, 0, 0.01f);
            yield return null;
        }
        audio.Stop();

    }
    IEnumerator IncreaseVolume(AudioSource audio)
    {
        audio.Play();
        while (audio.volume < 1)
        {
            audio.volume = Mathf.Lerp(audio.volume, 1, 0.01f);
            yield return null;
        }

    }
    public void PlayBoostSound()
    {
        audioSource[4].Play();
    }
}
