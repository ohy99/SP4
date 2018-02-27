using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelScript : MonoBehaviour {

    private Slider musicSlider;
    private Slider fxSlider;
    private Toggle musicToggle;
    private Toggle fxToggle;

    // Use this for initialization
    void Start()
    {
        this.transform.parent.gameObject.SetActive(false);

        Debug.Log("Hi");
        musicSlider = transform.GetChild(0).GetComponent<Slider>();

        musicSlider.value = SoundManager.Instance.musicVolume;

        fxSlider = transform.GetChild(1).GetComponent<Slider>();

        fxSlider.value = SoundManager.Instance.fxVolume;

        musicToggle = transform.GetChild(2).GetComponent<Toggle>();

        musicToggle.isOn = !SoundManager.Instance.musicSource.mute;

        fxToggle = transform.GetChild(3).GetComponent<Toggle>();

        fxToggle.isOn = !SoundManager.Instance.globalfxSource.mute;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AdjustMusicVolume()
    {
        SoundManager.Instance.musicSource.volume = musicSlider.value;
        SoundManager.Instance.musicVolume = musicSlider.value;
        Debug.Log(SoundManager.Instance.musicSource.volume);
    }

    public void AdjustFXVolume()
    {
        SoundManager.Instance.globalfxSource.volume = fxSlider.value;
        SoundManager.Instance.fxVolume = fxSlider.value;
    }

    public void ToggleMusic()
    {
        SoundManager.Instance.musicSource.mute = !musicToggle.isOn;
    }

    public void ToggleFx()
    {
        SoundManager.Instance.globalfxSource.mute = !fxToggle.isOn; 
    }

}
