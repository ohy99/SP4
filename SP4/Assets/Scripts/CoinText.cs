using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinText : MonoBehaviour {

    public int coinvalue;

    private Vector2 pos;
    private Vector2 size;

    private string text;

    private GUIContent content;
    private GUIStyle style;

    private int fontSize = 50;

    float moveSpd = 1.0f;

    Vector3 moveDir = new Vector3(0, 1, 0);
    // Use this for initialization
    void Start () {

        content = new GUIContent();
        style = new GUIStyle();

        text = "+" + coinvalue;

        content.text = text;

        style.normal.textColor = new Color(0.85f, 0.85f, 0.1f);

        Destroy(gameObject, 2.0f);
	}
	
    void OnGUI()
    {
        style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));

        size = style.CalcSize(content);

        GUI.Label(new Rect(pos, size), content, style);
    }

	// Update is called once per frame
	void Update () {
        transform.position += moveDir * Time.deltaTime * moveSpd;
        pos = Global.Instance.cam.WorldToScreenPoint(transform.position);
        pos.y = Screen.height - pos.y - size.y * 2.0f;
    }
}
