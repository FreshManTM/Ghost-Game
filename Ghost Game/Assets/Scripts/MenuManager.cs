using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioMixerGroup Mixer;

    [SerializeField] Slider gmSlider, mmSlider, efSlider;

    void Start()
    {
        gmSlider.value = PlayerPrefs.GetFloat("GameMusicV");
        efSlider.value = PlayerPrefs.GetFloat("EffectMusicV");
        mmSlider.value = PlayerPrefs.GetFloat("MenuMusicV");
    }
    public void LoadLevel(int number)
    {
        SceneManager.LoadScene(number);
    }
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("GameMusicV", gmSlider.value);
        PlayerPrefs.SetFloat("MenuMusicV", mmSlider.value);
        PlayerPrefs.SetFloat("EffectMusicV", efSlider.value);

    }

    public void ChangeMenuMusicVolume(float volume)
    {
        
        if (volume > 0.5f)
            Mixer.audioMixer.SetFloat("MenuMusicVolume", Mathf.Lerp(-40, 0, volume));
        else if (volume > 0.25f)
            Mixer.audioMixer.SetFloat("MenuMusicVolume", Mathf.Lerp(-60, 20, volume));
        else
            Mixer.audioMixer.SetFloat("MenuMusicVolume", Mathf.Lerp(-80, 80, volume));
        SaveSettings();

    }

    public void ChangeEffectVolume(float volume)
    {
        if (volume > 0.5f)
            Mixer.audioMixer.SetFloat("EffectVolume", Mathf.Lerp(-40, 0, volume));
        else if (volume > 0.25f)
            Mixer.audioMixer.SetFloat("EffectVolume", Mathf.Lerp(-60, 20, volume));
        else
            Mixer.audioMixer.SetFloat("EffectVolume", Mathf.Lerp(-80, 80, volume));
        SaveSettings();

    }
    public void ChangeGameVolume(float volume)
    {
        if (volume > 0.5f)
            Mixer.audioMixer.SetFloat("GameMusicVolume", Mathf.Lerp(-40, 0, volume));
        else if (volume > 0.25f)
            Mixer.audioMixer.SetFloat("GameMusicVolume", Mathf.Lerp(-60, 20, volume));
        else
            Mixer.audioMixer.SetFloat("GameMusicVolume", Mathf.Lerp(-80, 80, volume));
        SaveSettings();

    }
    public void Exit()
    {
        Application.Quit();
    }
    
}
