using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int number = 0;
    public bool isColliding = false;

    private MeshRenderer tileMeshRenderer;

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
        float timer = 0.0f;

        // color lerp within the given delay
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;

            tileMeshRenderer.materials[1].color = Color.Lerp(tileMeshRenderer.materials[1].color,Color.white, timer * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        tileMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
