using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;

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

    public GameObject pausePanel;

    public int resetTimer = 3;
    public int timer = 20;
    private float currentTime = 0f;

    // Executions
    private bool executeOnce = false;
    private bool isPlayerDead = false;

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
        Initialize();
        AudioManager.GetInstance().PlaySceneTrack(AudioManager.MusicTrack.BGM_StartScene, 0f, 0.5f);
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

        // Objective
        GameObject objective = Instantiate(ObjectivePrefab,
            new Vector3(ObjectiveSpawnLocation.x, roundNumber * Y_PositionOffset + 1,
                ObjectiveSpawnLocation.z), Quaternion.identity);

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
        timer = resetTimer;
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

    //--------------------------------- Objective and Collectibles ---------------------------------//

    /// <summary>
    /// Spawns Timer Collectible
    /// </summary>
    public void SpawnTimerCollectible(int tileNumber)
    {
        // Spawn Timer
        GameObject timer = Instantiate(TimerCollectiblePrefab,
            new Vector3(Tiles[tileNumber].transform.position.x, Tiles[tileNumber].transform.position.y + (roundNumber * Y_PositionOffset) + 2f,
                Tiles[tileNumber].transform.position.z), Quaternion.identity);
    }

    /// <summary>
    /// Collectible will add time
    /// </summary>
    public void AddTime(int timeToAdd)
    {
        timer += timeToAdd;
        StartCoroutine(UpdateTimerDisplay());
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
        player.gameObject.GetComponent<MovementComponent>().enabled = false;
        
        // show end screen panel
    }
}
