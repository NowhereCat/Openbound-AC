using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public AudioMixer audiomixer;

    public Slider masterAudioSlider;
    public Slider soundAudioSlider;
    public Slider chatterAudioSlider;
    public Slider musicAudioSlider;


    const float minAudio = -80f;
    const float maxAudio = 20f;

    // Start is called before the first frame update
    void Start()
    {
        masterAudioSlider.minValue = minAudio;
        masterAudioSlider.maxValue = maxAudio;

        soundAudioSlider.minValue = minAudio;
        soundAudioSlider.maxValue = maxAudio;

        chatterAudioSlider.minValue = minAudio;
        chatterAudioSlider.maxValue = maxAudio;

        musicAudioSlider.minValue = minAudio;
        musicAudioSlider.maxValue = maxAudio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterAudio()
    {
        audiomixer.SetFloat("MasterVol", masterAudioSlider.value);
    }

    public void SetSoundAudio()
    {
        audiomixer.SetFloat("MusicVol", soundAudioSlider.value);
    }

    public void SetChatterAudio()
    {
        audiomixer.SetFloat("ChatterVol", chatterAudioSlider.value);
    }

    public void SetMusicAudio()
    {
        audiomixer.SetFloat("MusicVol", musicAudioSlider.value);
    }
}
