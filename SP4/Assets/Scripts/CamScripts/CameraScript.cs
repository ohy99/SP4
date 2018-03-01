using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    GameObject player;

    public Transform playerTransform;

    void Awake()
    {
        Global.Instance.cam = this.gameObject.GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
        {
            //if (playerTransform == null)
            //    return;
            //transform.position = playerTransform.position + new Vector3(0, 0, -10);
            player = Global.Instance.player;
            return;
        }

        if(!player.activeSelf)
            ChangeTarget();

        transform.position = player.transform.position + new Vector3(0, 0, -10);
        //Debug.Log(transform.position + ", player:" +player.transform.position.x +"," +player.transform.position.y);

    }

    //change who the cam follow
    void ChangeTarget()
    {
        Debug.Log("Changing cam target");
        if (Global.Instance.listOfPlayer == null)
            return;

        GameObject[] playersList = Global.Instance.listOfPlayer;
        for(int i = 0; i < playersList.Length; ++i)
        {
            if(playersList[i].activeSelf)
            {
                player = playersList[i];
                break;
            }
        }
    }

}
