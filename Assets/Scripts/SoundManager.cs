using System;
using System.Collections;
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

    private void Start()
    {
        CheckSound();
    }

    public void ToggleSound()
    {
        int isSound = PlayerPrefs.GetInt("SoundState");
        bool soundEnabled = (isSound == 1); // Переводим значение int в bool

        soundEnabled = !soundEnabled; // Инвертируем состояние звука

        // Сохраняем новое состояние звука
        PlayerPrefs.SetInt("SoundState", soundEnabled ? 1 : 0);
        PlayerPrefs.Save();

        // Вызываем проверку звука
        CheckSound();
    }
    
    private void CheckSound()
    {
        if (!PlayerPrefs.HasKey("SoundState"))
        {
            PlayerPrefs.SetInt("SoundState", 1);
            PlayerPrefs.Save();
        }
        int isSound = PlayerPrefs.GetInt("SoundState");
        bool soundEnabled = (isSound == 1); // Переводим значение int в bool

        if (!soundEnabled)
        {
            AudioListener.volume = 0f; // Устанавливаем громкость на 0
            // Если есть другие компоненты или системы управления звуком, их также можно отключить
        }
        else
        {
            AudioListener.volume = 1f; // Устанавливаем громкость на максимальное значение
            // Включаем другие компоненты или системы управления звуком
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
