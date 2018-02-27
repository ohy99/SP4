using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenericSpawner : NetworkBehaviour
{
    GameObject map;



	// Use this for initialization
	void Start ()
    {
        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");
        else
            Debug.Log("SERVER ACTIVE");
    }

	// Update is called once per frame
	void Update ()
    {
    }

    public void Init(GameObject _map)
    {
        map = _map;
        //objectSpawnList = new List<GameObject>();
    }

    public void SpawnObject(int numSpawn, GameObject _go)
    {
        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");

        for (int i = 0; i < numSpawn; ++i)
        {
            Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
        Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
            var go = Instantiate(_go, pos, Quaternion.identity);

           NetworkServer.Spawn(go);
        }
    }

    public GameObject SpawnObject(Vector3 pos, GameObject _go)
    {
        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");

        var go = Instantiate(_go, pos, Quaternion.identity);
        NetworkServer.Spawn(go);

        return go;
    }

    //special case
    public void SpawnSpeedItem(Vector3 pos, GameObject _go,GameObject _parent, SpeedRoomItemSpawner script)
    {
        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");

        var go = Instantiate(_go, pos, Quaternion.identity);
        go.GetComponent<SpeedRoomItemScript>().SetSpawner(script);
        go.transform.SetParent(_parent.transform);
        NetworkServer.Spawn(go);
    }
}
