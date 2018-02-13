using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 10;
    public float rotateSpeed = 60;

    //JoyScript targetScript;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = new Vector3(0, 1, 0);
        Vector3 right = new Vector3(1, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += right * moveSpeed * Time.deltaTime;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 dir = new Vector2(mousePos.x - transform.position.x,
            mousePos.y - transform.position.y);

        transform.up = dir;

        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //transform.Rotate(new Vector3(0,0, angle) * Time.deltaTime * rotateSpeed);
    }
}
