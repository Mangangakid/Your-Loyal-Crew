using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class AudioController : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;
    public AudioMixer audioMixer;
    public AudioSource click;


    void Start()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            bool i = false;
            int j = PlayerPrefs.GetInt("Music");
            if (j == 1)
            {
                i = true;
            }
            musicChange(i);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        if (PlayerPrefs.HasKey("SFX"))
        {
            bool i = false;
            int j = PlayerPrefs.GetInt("SFX");
            if (j == 1)
            {
                i = true;
            }
            sfxChange(i);
        }
        else
        {
            PlayerPrefs.SetInt("SFX", 1);
        }
    }

    public void MusicToggle()
    {
        musicChange(musicToggle.isOn);
    }

    public void SfxToggle()
    {
        sfxChange(sfxToggle.isOn);
    }

    void musicChange(bool value)
    {
        musicToggle.isOn = value;
        if (value)
        {
            PlayerPrefs.SetInt("Music", 1);
            audioMixer.SetFloat("musicVol", 0f);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
            audioMixer.SetFloat("musicVol", -80f);
        }
    }

    void sfxChange(bool value)
    {
        sfxToggle.isOn = value;
        if (value)
        {
            PlayerPrefs.SetInt("SFX", 1);
            audioMixer.SetFloat("sfxVol", 0f);
        }
        else
        {
            PlayerPrefs.SetInt("SFX", 0);
            audioMixer.SetFloat("sfxVol", -80f);
        }
    }
    public void ButtonSound()
    {
        click.Play();
    }
}
