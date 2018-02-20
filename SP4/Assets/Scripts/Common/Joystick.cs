using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {
    
    Vector3 startPosition;
    RectTransform rectTransform;
    // Use this for initialization
    void Start () {
        //parent = transform.parent.gameObject;
    }
    void Awake()
    {
#if UNITY_WINDOWS
        //this.gameObject.SetActive(false);
#elif UNITY_ANDROID
        //this.gameObject.SetActive(true);
#endif   

        //parentTransform = GetComponentInParent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    public float GetXAxis()
    {
        float radius = rectTransform.rect.width * rectTransform.localScale.x;
        return transform.localPosition.x / radius;
    }
    public float GetYAxis()
    {
        float radius = rectTransform.rect.height * rectTransform.localScale.y;
        return transform.localPosition.y / radius;
    }

    public void OnDragEnter()
    {
        startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        //Debug.Log("WAD");
    }
    public void Dragging()
    {
        Vector3 newPosition = new Vector3(0, 0, 0);
#if UNITY_EDITOR || UNITY_WINDOWS
        //Vector3 screenPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition = new Vector3(Input.mousePosition.x - startPosition.x, 
            Input.mousePosition.y - startPosition.y, 0);

#elif UNITY_ANDROID
        Touch mytouch = Input.GetTouch(0);
        newPosition = new Vector3(mytouch.position.x - startPosition.x, mytouch.position.y - startPosition.y, 0);

#endif   
       
        //joyFG.rectTransform.localPosition = newPosition;
        transform.localPosition = newPosition;
        //direction = joyFG.rectTransform.localPosition;
        float radius = rectTransform.rect.width * rectTransform.localScale.x;
        if (transform.localPosition.sqrMagnitude > radius * radius)
        {
            transform.localPosition = transform.localPosition.normalized * radius;
        }
        //transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, 
        //    -rectTransform.rect.width * rectTransform.localScale.x, 
        //    rectTransform.rect.width * rectTransform.localScale.x),
        //    Mathf.Clamp(transform.localPosition.y,
        //    -rectTransform.rect.height * rectTransform.localScale.y,
        //    rectTransform.rect.height * rectTransform.localScale.y), 0);

    }
    public void StopDrag()
    {
        //joyFG.rectTransform.localPosition = new Vector3(0, 0, 1);
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
