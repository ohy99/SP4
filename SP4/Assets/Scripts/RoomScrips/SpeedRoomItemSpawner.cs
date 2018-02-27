using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpeedRoomItemSpawner : MonoBehaviour {

    public int numOfItemSpawned;

    private int numOfItemDestroyed;

    public int roomId;

    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject item;

    [SerializeField]
    GameObject genericSpawner;

    GameObject player;
    GameObject[] playersList;

    float spawnDelay = 3.0f;
    public int maxSpawns = 5;

    public bool spawnerActive;

    void Awake()
    {
        maxSpawns = Random.Range(5, 10);
        numOfItemSpawned = 0;
        numOfItemDestroyed = 0;
        spawnerActive = false;
    }

    // Use this for initialization
    void Start()
    {
        //playersList = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log("numPlayer: " + playersList.Length);
        //for (int i = 0; i < playersList.Length; i++)
        //{
        //    if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
        //    {
        //        player = playersList[i];
        //        break;
        //    }
        //}

        player = Global.Instance.player;

        if(player.GetComponent<NetworkIdentity>().isServer)
        {
            //send to all client the no of item spawned
            MessageHandler.Instance.SendNumberToSpawn_S2C(map.GetComponent<RoomScript>().GetRoomID(), maxSpawns, "speedRoom");
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator SpawnItem()
    {
        for (numOfItemSpawned = 0; numOfItemSpawned < maxSpawns; ++numOfItemSpawned)
        {
            yield return new WaitForSeconds(spawnDelay);
            //  Vector3 pos = new Vector3(Random.Range(map.transform.position.x + -map.transform.localScale.x * 0.5f + 2.5f, map.transform.position.x + map.transform.localScale.x * 0.5f - 2.5f),
            //Random.Range(map.transform.position.y + -map.transform.localScale.y * 0.5f + 2.5f, map.transform.position.y + map.transform.localScale.y * 0.5f - 2.5f), 0);
            //   GameObject temp = Instantiate(item, pos, Quaternion.identity);
            //   temp.GetComponent<SpeedRoomItemScript>().SetSpawner(this);
             //  temp.transform.SetParent(game.transform.parent);
            CallSpawner();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player") && !spawnerActive)
        {
            roomId = map.GetComponent<RoomScript>().GetRoomID();
            //Debug.Log("SpawnerActive: " + spawnerActive);
            spawnerActive = true;
            this.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine("SpawnItem");
        }
    }

    public void AddDestroyedObjectCount()
    {
        numOfItemDestroyed++;

        if (numOfItemDestroyed == maxSpawns)
            SendMessageUpwards("SpawnEnemies", maxSpawns);
    }

    void CallSpawner()
    {
        if(Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        {
            Vector3 pos = new Vector3(Random.Range(map.transform.position.x + -map.transform.localScale.x * 0.5f + 2.5f, map.transform.position.x + map.transform.localScale.x * 0.5f - 2.5f),
          Random.Range(map.transform.position.y + -map.transform.localScale.y * 0.5f + 2.5f, map.transform.position.y + map.transform.localScale.y * 0.5f - 2.5f), 0);
            genericSpawner.GetComponent<GenericSpawner>().Init(map);
            item.GetComponent<SpeedRoomItemScript>().roomID = roomId;
            genericSpawner.GetComponent<GenericSpawner>().SpawnSpeedItem(pos, item, map, this);
        }
        else
        {
            Debug.Log("not server/host");
        }
    }
}
