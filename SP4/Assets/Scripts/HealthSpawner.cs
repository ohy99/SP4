using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    int numOfActiveHealthPacks;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject healthPack;

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
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay && numOfActiveHealthPacks < maxSpawns )
        {
            Vector3 pos = new Vector3(Random.Range(-map.transform.localScale.x * 0.5f, map.transform.localScale.x * 0.5f),
            Random.Range(-map.transform.localScale.y * 0.5f, map.transform.localScale.y * 0.5f), 0);
            GameObject temp = Instantiate(healthPack, pos, Quaternion.identity);
            temp.GetComponent<HealthPackScript>().SetSpawner(this);
            ++numOfActiveHealthPacks;
            elapsedTime = 0.0f;
        }
    }

    public void RemoveOne() { --numOfActiveHealthPacks; }
}
