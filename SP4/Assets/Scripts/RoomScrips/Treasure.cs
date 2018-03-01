using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Treasure : MonoBehaviour {

    [SerializeField]
    GameObject TreasureKey;
    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject genericSpawner;

    bool textTrigger = false;

    int fontSize = 50;

    GUIStyle style;

    GUIContent content;

    float screenCenter;

    string text;

    Vector2 size;

    bool unlockingNow;
    bool isUnlocked;
    bool isEmpty;
    float unlockingAnimElapsed;
    float unlockAnimDuration;

    [SerializeField]
    bool OnTouchUnlock = false;

    // Use this for initialization
    void Start () {
        SendMessageUpwards("GenerateTreasureKey", TreasureKey);

        style = new GUIStyle();

        content = new GUIContent();

        isUnlocked = false;
        unlockingNow = false;
        isEmpty = false;
        unlockingAnimElapsed = 0.0f;
        unlockAnimDuration = 1.0f;
    }

    bool rotateLeft = true;
    float rotateSpd = 360.0f;
    float rotationLimit = 30.0f;
    float currRotation = 0.0f;
    [SerializeField]
    float coinOutVel = 0.3f;
    [SerializeField]
    int numOfCoinsOut = 5;
    [SerializeField]
    int coinValue = 20;

	// Update is called once per frame
	void Update () {

        if (unlockingNow && !isUnlocked)
        {
            unlockingAnimElapsed += Time.deltaTime;

            //SHAKE IT
            Quaternion quat = Quaternion.Euler(0, 0, (currRotation = Mathf.Clamp(currRotation + (rotateLeft ? -rotateSpd : rotateSpd) * Time.deltaTime, -rotationLimit, rotationLimit)));
            if (Mathf.Approximately(currRotation, (currRotation > 0.0f ? rotationLimit : -rotationLimit)))
                rotateLeft = !rotateLeft;
            gameObject.transform.rotation = quat;

            if (unlockingAnimElapsed >= unlockAnimDuration)
            {
                isUnlocked = true;
            }
        }
        if (isUnlocked && !isEmpty)
        {
            //for (int i = 0; i < numOfCoinsOut; ++i)
            //{
                //GameObject newCoin = Instantiate(coin, transform.position, Quaternion.identity);
                //Vector2 outDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                //outDir.Normalize();
                //Rigidbody2D rb = newCoin.GetComponent<Rigidbody2D>();
                ////rb.AddRelativeForce(new Vector2(outDir.x * coinOutForce, outDir.y * coinOutForce));
                //rb.velocity = new Vector2(outDir.x * coinOutVel, outDir.y * coinOutVel);
                //CoinValue cv = newCoin.GetComponent<CoinValue>();
                //cv.value = coinValue;
                
            //}

            if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
                genericSpawner.GetComponent<GenericSpawner>().SpawnCoins(coin, transform.position, coinOutVel, coinValue, numOfCoinsOut);

            isEmpty = true;
            
        }
	}

    void OnGUI()
    {
        style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));
        style.alignment = TextAnchor.UpperCenter;

        screenCenter = Screen.width * 0.5f;

        if (textTrigger)
        {
            text = "Find the key around the map to unlock this chest";
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, 0, size.x * 2.0f, size.y * 2.0f), text, style);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            if (col.transform.GetChild(0).tag.Equals("Key") || OnTouchUnlock)// && col.transform.GetChild(0).GetComponent<TreasureKey>().GetTreasureRoomID() == this.GetComponentInParent<RoomScript>().GetRoomID())
            {
                //InventoryManager.Instance.GetInventory("player").AddCurrency(100);
                //Destroy(this.gameObject);
                if (!OnTouchUnlock)
                    Destroy(col.transform.GetChild(0).gameObject);
                unlockingNow = true;
            }
            else if (!textTrigger)
            {
                textTrigger = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            if (textTrigger)
                textTrigger = false;
        }
    }
}
