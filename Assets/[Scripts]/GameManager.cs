using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Tiles")]
    public List<GameObject> TilePrefabs;
    public int tileIndex = 0;
    public Vector2 tileMapDimensions = Vector2.zero;
    public float PositionOffset = 6f;
    public float Y_PositionOffset = -20f;
    public int roundNumber = 0;
    public List<GameObject> Tiles;

    [Header("Objective")] 
    public GameObject ObjectivePrefab;
    public Vector3 ObjectiveSpawnLocation;

    [Header("Items")] 
    public GameObject TimerCollectiblePrefab;

    public GameObject TMP_PlusTimer;
        

    [Header("Deathplane")] 
    public GameObject DeathPlane;

    [Header("Player")]
    public PlayerController player;

    [Header("UI")] 
    public TextMeshProUGUI TMP_timer;

    [Header("Score")] 
    public TextMeshProUGUI TMP_Score;
    
    [Header("Others")]
    public GameObject pausePanel;

    [Header("Audio")] 
    public AudioClip ItemPickUp;
    public AudioClip ObjectiveClip;

    public int resetTimer = 3;
    public int timer = 20;
    private float currentTime = 0f;

    // Executions
    private bool executeOnce = false;
    private bool isPlayerDead = false;
    public int NumberOfObjectivesCollected = 0;
    public bool isObjectiveSpawned = false;

    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Data.Reset();
        Initialize();
        AudioManager.GetInstance().PlaySceneTrack(AudioManager.MusicTrack.BGM_StartScene, 0f, 0.2f);
        TMP_Score.text = Data.Score.ToString();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerDead)
        {
            TimerFunction();
        }

        if (player.isDying && !isPlayerDead)
        {
            isPlayerDead = true;
            PlayerDeadFunction();
        }
    }

    /// <summary>
    /// Initialize for game start
    /// </summary>
    public void Initialize()
    {
        tileIndex = 0;
        roundNumber = 0;
        player.GetComponent<Transform>().position = player.spawnPoint;
        GenerateTiles(tileIndex);
    }


    //-------------------------------------- Tile Generation --------------------------------------//

    /// <summary>
    /// Generate tiles of the given index
    /// </summary>
    /// <param name="index"></param>
    public void GenerateTiles(int index)
    {
        int number = 0;
        for (int i = 0; i < tileMapDimensions.x; i++)
        {
            for (int j = 0; j < tileMapDimensions.y; j++)
            {
                GameObject tile = Instantiate(TilePrefabs[index], new Vector3(i * PositionOffset, roundNumber * Y_PositionOffset, j * PositionOffset),
                    Quaternion.identity);
                tile.GetComponent<TileBehaviour>().number = number++;
                Tiles.Add(tile);
            }
        }

        // Death plane update
        Vector3 DeathPlanePosition = DeathPlane.transform.position;
        DeathPlane.transform.position = new Vector3(DeathPlanePosition.x, (roundNumber + 1) * Y_PositionOffset, DeathPlanePosition.z);
    }

    //-------------------------------------- Level Progression --------------------------------------//

    /// <summary>
    /// Timer Function, on end proceeds to next round
    /// </summary>
    private void TimerFunction()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 1f)
        {
            currentTime = 0f;
            timer--;
            TMP_timer.text = timer.ToString();

            if (timer <= 0 && !executeOnce)
            {
                executeOnce = true;
                //ProceedToNextRound();
                OutOfTime();
            }
        }
    }


    /// <summary>
    /// Proceed to next round
    /// </summary>
    public void ProceedToNextRound()
    {
        // increment tile, in case in the last part of list, reset
        tileIndex++;
        if (tileIndex >= TilePrefabs.Count)
        {
            tileIndex = 0;
        }

        // increment round
        roundNumber++;

        // reset timer
        timer += resetTimer;
        executeOnce = false;

        foreach (var tile in Tiles)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }

        Tiles.Clear();

        GenerateTiles(tileIndex);
    }

    /// <summary>
    /// Out of time function.. Game Over
    /// </summary>
    public void OutOfTime()
    {
        Data.isTimeOut = true;
        SceneManager.LoadScene((int)Scene.END);
    }

    //--------------------------------- Objective and Collectibles ---------------------------------//


    public void ObjectiveCollected(Objective objective)
    {
        NumberOfObjectivesCollected++;

        switch (NumberOfObjectivesCollected)
        {
            case 1:
                ProceedToNextRound();
                Destroy(objective.gameObject);
                isObjectiveSpawned = false;
                AudioManager.GetInstance().PlaySFX(player.gameObject.GetComponent<AudioSource>(), ObjectiveClip, 0.4f, false);
                break;

            case 2:
                ProceedToNextRound();
                Destroy(objective.gameObject);
                isObjectiveSpawned = false;
                AudioManager.GetInstance().PlaySFX(player.gameObject.GetComponent<AudioSource>(), ObjectiveClip, 0.4f, false);
                break;


            case 3:
                AudioManager.GetInstance().PlaySFX(player.gameObject.GetComponent<AudioSource>(), ObjectiveClip, 0.4f, false);
                Data.isVictory = true;
                SceneManager.LoadScene(2);
                break;

            default:
                Data.isVictory = false;
                SceneManager.LoadScene(2);
                break;
        }

    }


    /// <summary>
    /// Spawns Timer Collectible
    /// </summary>
    public void SpawnTimerCollectible(int tileNumber)
    {
        // Spawn Timer

        GameObject timer = Instantiate(TimerCollectiblePrefab,
            new Vector3(Tiles[tileNumber].transform.position.x, Tiles[tileNumber].transform.position.y  + 2f,
                Tiles[tileNumber].transform.position.z), Quaternion.identity);
    }

    /// <summary>
    /// Collectible will add time
    /// </summary>
    public void ItemCollected(int timeToAdd)
    {
        AudioManager.GetInstance().PlaySFX(player.gameObject.GetComponent<AudioSource>(),ItemPickUp,0.4f,false);

        // Time update
        timer += timeToAdd;
        StartCoroutine(UpdateTimerDisplay());

        // Score update
        Data.Score += 10;
        TMP_Score.text = Data.Score.ToString();


        if (Data.Score >= 100 * (roundNumber + 1))
        {
            if (!isObjectiveSpawned)
            {

                if ((roundNumber + 1) % 2 == 0)
                {
                    // Objective
                    GameObject objective = Instantiate(ObjectivePrefab,
                        new Vector3(0, roundNumber * Y_PositionOffset + 1,
                            0), Quaternion.identity);

                }
                else
                {
                    // Objective
                    GameObject objective = Instantiate(ObjectivePrefab,
                        new Vector3(ObjectiveSpawnLocation.x, roundNumber * Y_PositionOffset + 1,
                            ObjectiveSpawnLocation.z), Quaternion.identity);
                }


                isObjectiveSpawned = true;

            }
        }
    }

    /// <summary>
    /// Add +(x) timer UI
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTimerDisplay()
    {
        TMP_PlusTimer.SetActive(true);
        yield return new WaitForSeconds(1f);
        TMP_PlusTimer.SetActive(false);
    }

    //-------------------------------------- Player --------------------------------------//

    /// <summary>
    /// Player Dead function
    /// </summary>
    private void PlayerDeadFunction()
    {
        Cursor.visible = true;
        player.gameObject.GetComponent<MovementComponent>().enabled = false;
        
        // show end screen panel
        Data.isVictory = false;
        StartCoroutine(EndScreenCoroutineOnPlayerDeath());
        
    }

    IEnumerator EndScreenCoroutineOnPlayerDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }
}
