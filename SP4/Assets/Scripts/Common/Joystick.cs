using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {
    
    Vector3 startPosition;
    RectTransform rectTransform;

    [SerializeField]
    bool onInUnityEditor = true;
    int fingerID = -1;

    // Use this for initialization
    void Start () {
        //parent = transform.parent.gameObject;

#if UNITY_STANDALONE
        this.gameObject.SetActive(false);
        this.gameObject.transform.parent.gameObject.SetActive(false);
#elif UNITY_ANDROID
        this.gameObject.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(true);
#endif


#if UNITY_EDITOR
        this.gameObject.SetActive(onInUnityEditor);
        this.gameObject.transform.parent.gameObject.SetActive(onInUnityEditor);
#endif


        //parentTransform = GetComponentInParent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Awake()
    {

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
#if UNITY_EDITOR || UNITY_STANDALONE
        startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        //Debug.Log("WAD");
#elif UNITY_ANDROID
        Touch[] touches = Input.touches;
        Touch mytouch = Input.GetTouch(touches.Length - 1);//get the last touch
        fingerID = mytouch.fingerId;

        startPosition = new Vector3(mytouch.position.x, mytouch.position.y, 0);
#endif
    }
    public void Dragging()
    {
        Vector3 newPosition = new Vector3(0, 0, 0);
#if UNITY_EDITOR || UNITY_STANDALONE
        //Vector3 screenPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition = new Vector3(Input.mousePosition.x - startPosition.x, 
            Input.mousePosition.y - startPosition.y, 0);

#elif UNITY_ANDROID
        Touch mytouch = Input.GetTouch(0);
        Touch[] touches = Input.touches;
        foreach (Touch t in touches)
        {
            if (t.fingerId == fingerID)
            {
                mytouch = t;
                break;
            }
        }
       
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
        fingerID = -1;
    }
}
