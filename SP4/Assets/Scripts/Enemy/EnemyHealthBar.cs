using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

    float barDisplay = 0;
    Vector2 pos;
    Vector2 size = new Vector2(60, 20);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
    public Camera cam;

    private Health hpScript;

    // Use this for initialization
    void Start()
    {
        pos = new Vector2(transform.position.x, transform.position.y);

        hpScript = GetComponent<Health>();
    }

    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(pos, size));
        GUI.Box(new Rect(new Vector2(), size), progressBarEmpty);

        GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
        GUI.EndGroup();

        GUI.EndGroup();
    }

    // Update is called once per frame
    void Update()
    {
        pos = cam.WorldToScreenPoint(transform.position);

        pos.x -= size.x * 0.5f;
        pos.y = Screen.height - pos.y - size.y * 2.0f;

        barDisplay = hpScript.GetCurrHp() / hpScript.GetMaxHp();
    }
}
