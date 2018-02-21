using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRoomItemSpawner : MonoBehaviour {

    public int numOfItemSpawned;

    private int numOfItemDestroyed;

    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject item;

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
            Vector3 pos = new Vector3(Random.Range(map.transform.position.x + -map.transform.localScale.x * 0.5f + 2.5f, map.transform.position.x + map.transform.localScale.x * 0.5f - 2.5f),
         Random.Range(map.transform.position.y + -map.transform.localScale.y * 0.5f + 2.5f, map.transform.position.y + map.transform.localScale.y * 0.5f - 2.5f), 0);
            GameObject temp = Instantiate(item, pos, Quaternion.identity);
            temp.GetComponent<SpeedRoomItemScript>().SetSpawner(this);
            temp.transform.SetParent(this.transform.parent);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player") && !spawnerActive)
        {
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
}
