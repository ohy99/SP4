using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global> {

    [SerializeField]
    GameObject _player;
    [SerializeField]
    Camera _camera;
    [SerializeField]
    RoomGenerator _roomGen;

    private bool _victory;
    

    public GameObject player { get { return _player; } set { _player = value; } }

    public Camera cam { get { return _camera; } set { _camera = value; } }

    public bool victory { get { return _victory; } set { _victory = value; } }

    public GameObject boss;

    public RoomGenerator roomGen { get { return _roomGen; } set { _roomGen = value; } }

    public bool bossIsDead;

    // Use this for initialization
    void Start () {
        _victory = false;
	}
    void Awake()
    {
        bossIsDead = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
