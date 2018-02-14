using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    [SerializeField]
    float countDown = 0.2f;
    private bool isAttacking = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("COLLIDER_START");
        isAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            Debug.Log("minusing: " + countDown);
            countDown -= Time.deltaTime;
            //transform.Rotate(Vector3.forward * -1.5f * Mathf.Rad2Deg * Time.deltaTime);
            if (countDown <= 0)
            {
                Debug.Log("delete");
                gameObject.SetActive(false);
            }
    
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit");
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    public float GetCountDown()
    {
        return countDown;
    }

    public void SetCountDown(float _countDown)
    {
        countDown = _countDown;
    }
}
