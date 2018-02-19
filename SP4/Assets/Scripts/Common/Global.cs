using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global> {

    [SerializeField]
    GameObject _player;

    public GameObject player { get { return _player; } private set { } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
