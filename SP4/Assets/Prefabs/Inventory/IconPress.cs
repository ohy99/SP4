using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconPress : MonoBehaviour {

    [System.NonSerialized]
    public bool doubleClicked = false;
    [System.NonSerialized]
    public bool isClicked = false;

    [System.NonSerialized]
    public float clickElapsed = 0.0f;
    float doubleClickDelay = 0.5f;

    [System.NonSerialized]
    public int id = 0;

    // Use this for initialization
    void Start () {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });

        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        eventTrigger.triggers.Add(entry);
    }
	
	// Update is called once per frame
	void Update () {
		if (isClicked)
        {
            clickElapsed += Time.deltaTime;
            if (clickElapsed >= doubleClickDelay)
            {
                Reset();
            }
        }
	}

    public void OnPointerDownDelegate(PointerEventData data)
    {
        if (!isClicked)
        {
            isClicked = true;
            clickElapsed = 0.0f;
            return;
        }

        //clicked second time
        doubleClicked = true;

    }

    public void Reset()
    {
        clickElapsed = 0.0f;
        isClicked = false;
        doubleClicked = false;
    }
}
