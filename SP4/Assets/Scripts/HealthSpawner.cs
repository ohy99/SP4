using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthSpawner : MonoBehaviour
{
    int numOfActiveHealthPacks;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject healthPack;
    [SerializeField]
    GameObject genericSpawner;

    float elapsedTime;
    [SerializeField]
    float spawnDelay = 5.0f;
    [SerializeField]
    int maxSpawns = 5;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.Instance.player)
            return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay && numOfActiveHealthPacks < maxSpawns )
        {
            if(Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            {
                Vector3 pos = new Vector3(Random.Range(-map.transform.localScale.x * 0.5f, map.transform.localScale.x * 0.5f),
        Random.Range(-map.transform.localScale.y * 0.5f, map.transform.localScale.y * 0.5f), 0);
                genericSpawner.GetComponent<GenericSpawner>().SpawnObject(pos, healthPack);
            }

            //GameObject temp = Instantiate(healthPack, pos, Quaternion.identity);
            //temp.GetComponent<HealthPackScript>().SetSpawner(this);
            ++numOfActiveHealthPacks;
            elapsedTime = 0.0f;
        }
    }

    public void RemoveOne() { --numOfActiveHealthPacks; }
}
