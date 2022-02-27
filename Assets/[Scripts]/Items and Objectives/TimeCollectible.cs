using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollectible : MonoBehaviour
{

    public int timeToAdd = 1;
    public float lifeSpan = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f,180 * Time.deltaTime,0f),Space.Self);
    }


    private void OnTriggerEnter(Collider other)
    {
        GameManager.GetInstance().AddTime(timeToAdd);
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject);
    }


    /// <summary>
    /// Modify time to add
    /// </summary>
    /// <param name="time"></param>
    public void SetTimeToAdd(int time)
    {
        timeToAdd = time;
    }

    /// <summary>
    /// Destroy object after x seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
