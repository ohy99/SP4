using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    PlayerShoot playerShoot;
    [SerializeField]
    Joystick moveJoy;
    [SerializeField]
    UnityEngine.UI.Text debugText;

    public float moveSpeed = 10;
    public float rotateSpeed = 60;

    enum CONTROLTYPE
    {
        KEYBOARD,
        GAMEPAD,
        MOBILE,
    }
    CONTROLTYPE controlType = CONTROLTYPE.KEYBOARD;
    //JoyScript targetScript;

    // Use this for initialization
    void Start () {
        Reset();
	}

    // Update is called once per frame
    void Update()
    {
        // networking stuff
        //if (!isLocalPlayer)
        //    return;

        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        //    ChangeControlType(CONTROLTYPE.KEYBOARD);
        //else if (!Mathf.Approximately(Input.GetAxis("LeftHorizontal"), 0.0f) || !Mathf.Approximately(Input.GetAxis("LeftVertical"), 0.0f))
        //    ChangeControlType(CONTROLTYPE.GAMEPAD);
        //else if (!Mathf.Approximately(moveJoy.GetXAxis(), 0.0f) || !Mathf.Approximately(moveJoy.GetYAxis(), 0.0f))
        //    ChangeControlType(CONTROLTYPE.MOBILE);

        switch (controlType)
        {
            case CONTROLTYPE.GAMEPAD:
                MoveOnGamePad();
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    ChangeControlType(CONTROLTYPE.KEYBOARD);
                else if (!Mathf.Approximately(moveJoy.GetXAxis(), 0.0f) || !Mathf.Approximately(moveJoy.GetYAxis(), 0.0f))
                    ChangeControlType(CONTROLTYPE.MOBILE);
                break;
            case CONTROLTYPE.MOBILE:
                MoveOnMobile();
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    ChangeControlType(CONTROLTYPE.KEYBOARD);
                else if (!Mathf.Approximately(Input.GetAxis("LeftHorizontal"), 0.0f) || !Mathf.Approximately(Input.GetAxis("LeftVertical"), 0.0f))
                    ChangeControlType(CONTROLTYPE.GAMEPAD);
                break;
            default:
                MoveOnKeyboard();
                if (!Mathf.Approximately(Input.GetAxis("LeftHorizontal"), 0.0f) || !Mathf.Approximately(Input.GetAxis("LeftVertical"), 0.0f))
                {
                    //Debug.Log("Horizontal " + Input.GetAxis("Horizontal") + " Vertical " + (Input.GetAxis("Vertical")));
                    ChangeControlType(CONTROLTYPE.GAMEPAD);
                }

                if (moveJoy == null)
                    return;

                else if (!Mathf.Approximately(moveJoy.GetXAxis(), 0.0f) || !Mathf.Approximately(moveJoy.GetYAxis(), 0.0f))
                    ChangeControlType(CONTROLTYPE.MOBILE);
                break;
        }

        if (debugText)
            debugText.text = controlType.ToString();
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //transform.Rotate(new Vector3(0,0, angle) * Time.deltaTime * rotateSpeed);
    }

    public void Reset()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.up = new Vector3(0, 1, 0);
    }

    void MoveOnKeyboard()
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
    }

    void MoveOnGamePad()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            //Debug.Log(names[x] + " Len: " + names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                //PS4_Controller = 1;
                //Xbox_One_Controller = 0;
            }
            else if (names[x].Length == 33 || names[x].Length == 22)
            {
                //XBOX ONE CONTROLLER || XBOX BLUETOOTH GAMEPAD
                //print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                //PS4_Controller = 0;
                //Xbox_One_Controller = 1;

                if (!Mathf.Approximately(Input.GetAxis("RightHorizontal"), 0.0f) || !Mathf.Approximately(Input.GetAxis("RightVertical"), 0.0f))
                {
                    float rhValue = Input.GetAxis("RightHorizontal");
                    float rvValue = Input.GetAxis("RightVertical");
                    Vector3 shootDir = new Vector3(rhValue, rvValue, 0);
                    //gameObject.transform.position += moveDir * moveSpeed * Time.deltaTime;
                    transform.up = shootDir;

                }

                float hValue = Input.GetAxis("LeftHorizontal");
                float vValue = Input.GetAxis("LeftVertical");
                Vector3 moveDir = new Vector3(hValue, vValue, 0);
                gameObject.transform.position += moveDir * moveSpeed * Time.deltaTime;
            }

        }

    }

    void MoveOnMobile()
    {
        float hValue = moveJoy.GetXAxis();
        float vValue = moveJoy.GetYAxis();
        Vector3 moveDir = new Vector3(hValue, vValue, 0);
        gameObject.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void ChangeControlType(CONTROLTYPE type)
    {
        this.controlType = type;
        if (playerShoot)
            playerShoot.SetControlType((int)type);
    }
}
