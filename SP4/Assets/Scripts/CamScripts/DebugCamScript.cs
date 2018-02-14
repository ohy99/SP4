using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamScript : MonoBehaviour {

    [SerializeField]
    float offset = 5;
    [SerializeField]
    Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float w = 0;
        float h = 0;
        cam.transform.position = RoomGenerator.Instance.GetMidPoint(out w, out h);
        cam.orthographicSize = (w > h? w * 0.5f: h * 0.5f) + offset;
	}
}
