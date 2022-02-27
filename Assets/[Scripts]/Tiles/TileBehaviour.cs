using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int number = 0;
    public bool isColliding = false;
    public float delayTimer = 2f;
    public float rateOfColorChange = 0.1f;

    private MeshRenderer tileMeshRenderer;

    private bool isTimerCollSpawned = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Disappear(GameManager.GetInstance().roundNumber + 1));
            isColliding = true;
            if (!isTimerCollSpawned)
            {
                isTimerCollSpawned = true;
                StartCoroutine(SpawnCollectible());
            }
                
        }
    }


    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }

    /// <summary>
    /// Make the tile disappear
    /// </summary>
    /// <param name="roundNumber"></param>
    /// <returns></returns>
    IEnumerator Disappear(int roundNumber)
    {
        float timer = 0.0f;

        //delayTimer *= roundNumber;

        // color lerp within the given delay
        while (timer < delayTimer * 0.25)
        {
            timer += Time.deltaTime;

            tileMeshRenderer.materials[1].color = Color.Lerp(tileMeshRenderer.materials[1].color,Color.white, rateOfColorChange * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(delayTimer);
        Destroy(gameObject);
    }


    /// <summary>
    /// Spawn collectible after 1 second
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCollectible()
    {
        yield return new WaitForSeconds(1f);
        GameManager.GetInstance().SpawnTimerCollectible(number);
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
