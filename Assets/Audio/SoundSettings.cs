using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float value)
    {
        if (value < 1)
        {
            value = 0.0001f;
        }

        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedMasterValue", value);
        mixer.SetFloat("MasterVolume", Mathf.Log10(value / 100f) * 20f);
    }

    public void SetVolumeFromSlider()
    { 
        SetVolume(slider.value);
    }

    public void RefreshSlider(float value)
    {
        slider.value = value;
    }
}
