using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    public AudioSource musicSource;
    public AudioSource globalfxSource;
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    public float musicVolume = 0.0f;
    public float fxVolume = 0.0f;

	// Use this for initialization
	void Awake () {

        musicSource = this.gameObject.AddComponent<AudioSource>();
        globalfxSource = this.gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;

        AudioClip BGM = (AudioClip)Resources.Load("Music/bensound-creepy");
        musicSource.clip = BGM;
        musicSource.Play();

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        fxVolume = PlayerPrefs.GetFloat("FXVolume", 1.0f);

        musicSource.mute = System.Convert.ToBoolean(PlayerPrefs.GetInt("MusicToggle", 1));
        globalfxSource.mute = System.Convert.ToBoolean(PlayerPrefs.GetInt("FXToggle", 1));

        musicSource.volume = musicVolume;
        globalfxSource.volume = fxVolume;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PlaySound(AudioClip clip)
    {
        globalfxSource.clip = clip;

        globalfxSource.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        globalfxSource.PlayOneShot(clip);
    }

    public void PlayRandomSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitch, highPitch);

        globalfxSource.pitch = randomPitch;

        globalfxSource.clip = clips[randomIndex];

        globalfxSource.Play();
    }
}
