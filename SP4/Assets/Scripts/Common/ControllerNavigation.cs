using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerNavigation : MonoBehaviour {

    [SerializeField]
    public Selectable firstSelectable;
    [SerializeField]
    Selectable backButton;

    [SerializeField]
    GameObject scrollPanel;

    Selectable curr;
    bool hasChanged;

	// Use this for initialization
	void Start () {
        hasChanged = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (curr)
        {
            //if is a slider then change value using horizontal
            Slider slider = curr.GetComponent<Slider>();
            if (slider)
            {
                if (Input.GetAxis("LeftHorizontal") > 0.25f)
                {
                    slider.value += 0.01f;
                }
                else if (Input.GetAxis("LeftHorizontal") < -0.25f)
                {
                    slider.value -= 0.01f;
                }
            }
        }
        if (scrollPanel)
        {
                if (Input.GetAxis("RightVertical") > 0.25f)
                {
                    scrollPanel.transform.position += new Vector3(0, -10, 0);//panel shift down
                }
                else if (Input.GetAxis("RightVertical") < -0.25f)
                {
                    scrollPanel.transform.position += new Vector3(0, 10, 0);//panel shift up
                }
            
        }


        if (!hasChanged)
        {
            if (Input.GetAxis("LeftHorizontal") > 0.25f)
            {
                if (!curr)
                    curr = firstSelectable;
                else
                {
                    Selectable find = curr.FindSelectableOnRight();
                    if (find)
                        curr = find;
                }
                hasChanged = true;
            }
            else if (Input.GetAxis("LeftHorizontal") < -0.25f)
            {
                if (!curr)
                    curr = firstSelectable;
                else
                {
                    Selectable find = curr.FindSelectableOnLeft();
                    if (find)
                        curr = find;
                }
                hasChanged = true;
            }
            else if (Input.GetAxis("LeftVertical") > 0.25f)
            {
                if (!curr)
                    curr = firstSelectable;
                else
                {
                    Selectable find = curr.FindSelectableOnUp();
                    if (find)
                        curr = find;
                }
                hasChanged = true;
            }
            else if (Input.GetAxis("LeftVertical") < -0.25f)
            {
                if (!curr)
                    curr = firstSelectable;
                else
                {
                    Selectable find = curr.FindSelectableOnDown();
                    if (find)
                        curr = find;
                }
                hasChanged = true;
            }
        }
        else if (hasChanged && Mathf.Approximately(Input.GetAxis("LeftHorizontal"), 0.0f) && Mathf.Approximately(Input.GetAxis("LeftVertical"), 0.0f))
        {
            hasChanged = false;
        }

        if (curr)
        {
            curr.Select();
            Debug.Log("CurrSelected: " + curr);
        }
        //Debug.Log(this.gameObject + " Update");

        if (Input.GetButtonDown("B"))
        {
            if (backButton)
            {
                backButton.Select();
            }
        }
    }


}
