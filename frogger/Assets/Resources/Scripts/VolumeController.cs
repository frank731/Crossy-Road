using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public Slider slider;
    public AudioMixer audioMixer;
    public string exposedParamName;

    private void Awake()
    {
        slider.value = PlayerPrefs.GetFloat(exposedParamName, 0.5f);
        //SetVolume(slider.value);
    }
    private void OnDisable()
    {
        //calls on start disable
        SetVolume(slider.value); 
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat(exposedParamName, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(exposedParamName, value);
    }
}
