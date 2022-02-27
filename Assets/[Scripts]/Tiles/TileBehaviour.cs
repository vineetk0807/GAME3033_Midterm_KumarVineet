using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public bool isColliding = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Disappear());
            isColliding = true;
        }
    }


    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }

    IEnumerator Disappear()
    {
        Debug.Log("Disappearing in 2");
        yield return new WaitForSeconds(2f);
        Debug.Log("Disappeared!");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
