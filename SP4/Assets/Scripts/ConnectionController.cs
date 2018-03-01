using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour
{
    NetworkManager manager;

    [SerializeField]
    GameObject inputField;
    //public string _networkAddress;
    string _networkAddress;

    public Text text;

    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        NetworkManager.singleton.networkAddress = "localhost";
    }
	
	// Update is called once per frame
	void Update ()
    {
        //manager.networkAddress = inputField.text;
        //Debug.Log(manager.networkAddress);

        //this is for the ip -> manager.networkAddress
        //if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        //{
        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        manager.StartServer();
        //    }
        //    if (Input.GetKeyDown(KeyCode.H))
        //    {
        //        manager.StartHost();
        //    }
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        manager.StartClient();
        //    }
        //}

        //if (NetworkServer.active) //showing port
        //{
        //    GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
        //    ypos += spacing;
        //}
        //if (NetworkClient.active) //showing adress & port
        //{
        //    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
        //    ypos += spacing;
        //}

        //if (NetworkClient.active && !ClientScene.ready) //this is to add player on hold till some1 host
        //{
        //    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
        //    {
        //        ClientScene.Ready(manager.client.connection);

        //        if (ClientScene.localPlayers.Count == 0)
        //        {
        //            ClientScene.AddPlayer(0);
        //        }
        //    }
        //    ypos += spacing;
        //}
    }

    public void OnClickHost()
    {
        Debug.Log("on_click_host");
        if (!NetworkServer.active)
        {
            Debug.Log("start_host");
            NetworkManager.singleton.StartHost();
        }
        else
            Debug.Log("Already have a server/host");
    }

    public void OnClickClient()
    {
        if(!NetworkClient.active)
            NetworkManager.singleton.StartClient();
        else
            Debug.Log("No server/host");
    }

    public void OnTypeAdress(Text _networkAddress)
    {
        NetworkManager.singleton.networkAddress = _networkAddress.text;
        Debug.Log(NetworkManager.singleton.networkAddress);
    }

    //private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    //{
    //    text.GetComponent<Text>().text = string.Format("{0}, {1}, {2}", condition, stackTrace, type);
    //}
}
