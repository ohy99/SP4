using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    DIRECTION side;
    [SerializeField]
    GameObject LeftDoor;
    [SerializeField]
    GameObject RightDoor;
    BoxCollider2D doorCollider;

    [SerializeField]
    bool isLocked = true;

    private float renderDepthOffset = -0.1f;

    // Use this for initialization
    void Start() {

        doorCollider = gameObject.GetComponent<BoxCollider2D>();

        if (LeftDoor == null || RightDoor == null)
            return;

        LeftDoor.transform.localScale = new Vector3(0.5f, 0.5f);
        RightDoor.transform.localScale = new Vector3(0.5f, 0.5f);

        if (Mathf.Approximately(this.transform.parent.localPosition.x,0.0f))
            side = (this.transform.parent.localPosition.y < 0.0f ? DIRECTION.DOWN : DIRECTION.UP);
        else
            side = (this.transform.parent.localPosition.x < 0.0f ? DIRECTION.LEFT : DIRECTION.RIGHT);

        if (isLocked)
        {
            LockDoor();
        }
        else
        {
            UnlockDoor();
        }
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log("enabled: " + doorCollider.enabled);

    }

    public void ToggleLock(bool islocking)
    {
        //Debug.Log(islocking);
        if (islocking)
        {
            LockDoor();
        }
        else
        {
            UnlockDoor();
        }
    }

    private void LockDoor()
    {
        //Debug.Log(doorCollider + " Locked");
        LeftDoor.transform.localPosition = new Vector3(-0.25f, 0.0f, renderDepthOffset);
        RightDoor.transform.localPosition = new Vector3(0.25f, 0.0f, renderDepthOffset);
        //doorCollider.enabled = true;
        this.isLocked = true;
        //Debug.Log("isLocked: " + isLocked);
        
    }
    private void UnlockDoor()
    {
        LeftDoor.transform.localPosition = new Vector3(-0.75f, 0.0f, renderDepthOffset);
        RightDoor.transform.localPosition = new Vector3(0.75f, 0.0f, renderDepthOffset);
        //doorCollider.enabled = false;
        this.isLocked = false;
        //Debug.Log("isLocked: " + isLocked);
        //Debug.Log(doorCollider + " UnLocked");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.tag.Equals("Player"))
            return;

        if (this.isLocked)
            return;
        //RoomGenerator.Instance.GenerateRoom(side);
        //Debug.Log("Sending Message Upwards To Generate Room at " + side);
        gameObject.SendMessageUpwards("GenerateRoom", side);
        doorCollider.enabled = false;
    }

    //only call this when u have to off it
    public void OffTriggerBox()
    {
        if (doorCollider == null)
            doorCollider = gameObject.GetComponent<BoxCollider2D>();
        doorCollider.enabled = false;
    }
    public void OnTriggerBox()
    {
        if (doorCollider == null)
            doorCollider = gameObject.GetComponent<BoxCollider2D>();
        doorCollider.enabled = true;
    }

    public bool GetIsLocked()
    {
        return isLocked;
    }
    public bool GetHasTriggerBox()
    {
        if (doorCollider == null)
            doorCollider = gameObject.GetComponent<BoxCollider2D>();
        return doorCollider.enabled; }

}
