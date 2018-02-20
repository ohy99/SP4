using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global> {

    [SerializeField]
    GameObject _player;
    [SerializeField]
    Camera _camera;

    public GameObject player { get { return _player; } private set { } }

    public Camera cam { get { return _camera; } private set { } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
