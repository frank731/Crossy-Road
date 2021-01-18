using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using BayatGames.SaveGameFree;

public class GameManager : Singleton<GameManager>
{
    public int loadedRows;
    public Camera mainCamera;
    public GameObject[] cars;
    public GameObject[] grass;
    public GameObject[] roads;
    public GameObject[] water;
    public GameObject deathScreen;
    public TMPro.TextMeshProUGUI deathScoreText;
    public PlayerController playerController;
    public GameObject[][] rows;
    public GameObject settingsScreen;
    public GameObject[] players;
    public List<LinearMovementObject> carsLMO = new List<LinearMovementObject>();
    public List<SpawningRow> roadsSpawn = new List<SpawningRow>();
    public bool playTimeMan = false;
    public GameObject transition;
    public TransitionController transitionController;

    private int rowTypeCount;
    private Vector3 nextRowLocation = new Vector3(0, -6.5f, -31.5f);
    private Vector3 firstRowLocation;
    private bool lastWaterRowFlipped = false;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        try
        {
            transitionController = GameObject.FindGameObjectWithTag("Transition").transform.GetChild(0).GetComponent<TransitionController>();
        }
        catch
        {
            GameObject t = Instantiate(transition);
            transitionController = t.transform.GetChild(0).GetComponent<TransitionController>();
        }
        rows = new GameObject[][]{ grass, roads, water };
        rowTypeCount = rows.Length;
        firstRowLocation = nextRowLocation;
        GameObject player = Instantiate(players[SaveGame.Load<int>("Player Choice")], new Vector3(0, 0, 0), transform.rotation);
        playerController = player.GetComponent<PlayerController>();
        Physics.IgnoreLayerCollision(7, 9, false);
    }
    private void Start()
    {
        transitionController.FadeFromWhite(3f);
        for(int i = 0; i < loadedRows; i++)
        {
            CreateRowFront();
        }
    }

    public void CreateRowFront()
    {
        if(nextRowLocation.z != 0.5f)
        {
            CreateRow(nextRowLocation);
        }
        nextRowLocation.Set(nextRowLocation.x, nextRowLocation.y, nextRowLocation.z + 8);
    }

    public void CreateRowBack()
    {
        CreateRow(firstRowLocation);
        firstRowLocation.Set(firstRowLocation.x, firstRowLocation.y, firstRowLocation.z - 8);
    }

    private void CreateRow(Vector3 location)
    {
        int index = Random.Range(0, rowTypeCount);
        ref GameObject[] newRows = ref rows[index];
        GameObject newRoom = Instantiate(newRows[Random.Range(0, newRows.Length)], location, transform.rotation);
        if (Random.Range(0, 2) == 0)
        {
            SpawningRow spawningRow = newRoom.transform.GetComponent<SpawningRow>();
            if (spawningRow != null)
            {
                if(index == 2) //check if next row is water row
                {
                    if (!lastWaterRowFlipped)
                    {
                        spawningRow.Flip(); //flip water rows if next to each other to make game possible
                        lastWaterRowFlipped = true;
                    }
                    else
                    {
                        lastWaterRowFlipped = false;
                    }
                }
                else
                {
                    spawningRow.Flip();
                }
            }
        }
        else if(index == 2 && !lastWaterRowFlipped)
        {
            newRoom.GetComponent<SpawningRow>().Flip(); //flips water row if last row was an unflipped water row
            lastWaterRowFlipped = true;
        }
        else
        {
            lastWaterRowFlipped = false;
        }
    }

    public void Restart()
    {
        transitionController.FadeToWhite(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnDie()
    {
        long highScore;
        if(SaveGame.Exists("High Score"))
        {
            highScore = SaveGame.Load<long>("High Score");
        }
        else
        {
            highScore = SaveGame.Load<long>("High Score");
        }
   
        if(playerController.score > highScore)
        {
            highScore = playerController.score;
            SaveGame.Save("High Score", highScore);
        }
        deathScoreText.text = "Score: " + playerController.score.ToString() + "\nHigh Score: " + highScore;
        deathScreen.SetActive(true);
    }

    private void Update()
    {
        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && !playerController.isDead)
        {
            if (settingsScreen.activeSelf)
            {
                settingsScreen.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                settingsScreen.SetActive(true);
                Time.timeScale = 0;
            }
            
        }
    }
}
