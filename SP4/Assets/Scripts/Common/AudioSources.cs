using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSources : MonoBehaviour {

    AudioSource[] components;

    void Awake()
    {
        components = GetComponents<AudioSource>();

        SoundManager.Instance.musicSource = components[0];
        SoundManager.Instance.globalfxSource = components[1];
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
