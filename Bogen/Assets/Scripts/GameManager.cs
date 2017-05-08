using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public GameObject[] level;
    int currentLevel = 0;
    public bool gameActive = false;

    public string sceneName;
    public Zombie zombiePrefab;
    public Fence fence;
    public Player player;
    public Bow bow;
    public UI ui;

    [Range(0, 50)]
    public float spawnRadius = 25;
    [Range(0, 20)]
    public float wavePause = 5;

    public Wave[] waves;

    private List<Zombie> zombies;
    private List<Zombie> zombiesToKill;

    private bool spawning = false;
    private int waveId = -1;

    public bool gameOver = false;

    private int arrowsShot = 0;
    private int ZombiesKilled = 0;
    private float timeNeeded = 0;
    private float startTime = 0;
    private float endTime = 0;
    public int zombieHit;
    public int zombieKill;
    private int offsetArrowShot = 0;
    int highscoreCounter = 11;

    public GameObject HighscoreBoard; 

    private int[] highscore;

    void Start()
    {
        NextLevel(0);
        highscore = new int[11];
        HighscoreBoard.SetActive(false);
    }

    private void Awake()
    {
        zombiesToKill = new List<Zombie>();
        zombies = new List<Zombie>();
    }

    private void LateUpdate()
    {
        ui.Health = player.health;
        if (gameActive == true)
        {
            for (int i = zombiesToKill.Count - 1; i >= 0; i--)
            {
                Zombie zombie = zombiesToKill[i];
                if (!zombie.Alive) zombiesToKill.Remove(zombie);
            }

            if (!spawning && !gameOver && zombiesToKill.Count <= 0)
            {
                waveId++;
                if (waveId < waves.Length) StartCoroutine(SpawnWave(waves[waveId]));
                else Win("You won! Congratulations!");
            }

            if (player.health <= 0 && !gameOver) GameOver("You're dead. :[");
        }

        arrowsShot = bow.arrowsShot - offsetArrowShot;

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameOver("You're dead. :[");
            //ReloudScene();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.SetInt(("highscore_0"), 0);
            PlayerPrefs.SetInt(("highscore_1"), 0);
            PlayerPrefs.SetInt(("highscore_2"), 0);
            PlayerPrefs.SetInt(("highscore_3"), 0);
            PlayerPrefs.SetInt(("highscore_4"), 0);
            PlayerPrefs.SetInt(("highscore_5"), 0);
            PlayerPrefs.SetInt(("highscore_6"), 0);
            PlayerPrefs.SetInt(("highscore_7"), 0);
            PlayerPrefs.SetInt(("highscore_8"), 0);
            PlayerPrefs.SetInt(("highscore_9"), 0);

            Debug.Log(
    "Highscore_0 : " + PlayerPrefs.GetInt("highscore_0") + "\n" +
    "Highscore_1 : " + PlayerPrefs.GetInt("highscore_1") + "\n" +
    "Highscore_2 : " + PlayerPrefs.GetInt("highscore_2") + "\n" +
    "Highscore_3 : " + PlayerPrefs.GetInt("highscore_3") + "\n" +
    "Highscore_4 : " + PlayerPrefs.GetInt("highscore_4") + "\n" +
    "Highscore_5 : " + PlayerPrefs.GetInt("highscore_5") + "\n" +
    "Highscore_6 : " + PlayerPrefs.GetInt("highscore_6") + "\n" +
    "Highscore_7 : " + PlayerPrefs.GetInt("highscore_7") + "\n" +
    "Highscore_8 : " + PlayerPrefs.GetInt("highscore_8") + "\n" +
    "Highscore_9 : " + PlayerPrefs.GetInt("highscore_9") + "\n");
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        spawning = true;

        ui.Message = "Level " + (waveId + 1);
        ui.ShowMessage();
        yield return new WaitForSeconds(wavePause);
        ui.HideMessage();

        foreach (Zombie zombie in zombies) Destroy(zombie.gameObject);
        zombies = new List<Zombie>();

        int count = 0;
        while (count < wave.enemyCount)
        {
            zombiesToKill.Add(SpawnZombie());
            count++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        spawning = false;
    }

    private Zombie SpawnZombie()
    {
        int id = Random.Range(0, fence.wallCount);

        float step = 2 * Mathf.PI / fence.wallCount;
        float angle = step * id + Random.Range(0, step);
        Zombie newZombie = Instantiate<Zombie>(zombiePrefab, transform);
        newZombie.transform.localPosition = (new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle))) * spawnRadius;
        newZombie.transform.localEulerAngles = Vector3.up * (angle * Mathf.Rad2Deg + 180);
        FenceWall wall = fence.Walls[id].GetComponent<FenceWall>();
        if (wall.Alive) newZombie.targets.Add(wall);
        newZombie.targets.Add(player);
        zombies.Add(newZombie);
        return newZombie;
    }

    public void NextLevel(int levelCounter)
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (i == levelCounter)
            {
                level[i].SetActive(true);
            }
            else
            {
                level[i].SetActive(false);
            }
        }
    }

    private void GameOver(string msg)
    {
        bow.gameObject.SetActive(false);
        msg = msg + "\n Your Score: " + GetScore();
        StartCoroutine(ui.DisplayMessage(msg, 3));
        gameOver = true;
        //GetScore();
    }

    private void Win(string msg)
    {
        bow.gameObject.SetActive(false);
        msg = msg + "\n Your Score: " + GetScore();
        StartCoroutine(ui.DisplayMessage(msg, 3));
        gameOver = true;
        //GetScore();
    }



    public void StartTimer()
    {
        startTime = Time.time;
        offsetArrowShot = bow.arrowsShot;
        zombieHit = 0;
    }

    private float GetScore()
    {
        int score = 0;
        endTime = Time.time;
        int time = (int)(endTime - startTime);
        //Debug.Log(time);
        float percentage = (float)zombieHit / ((float)arrowsShot);
        score = (int)((percentage * 10) * (600 / time) * zombieKill);
        Debug.Log(score);
        SetScore(score);
        
        Debug.Log(
            "Highscore_0 : " + PlayerPrefs.GetInt("highscore_0") + "\n" +
            "Highscore_1 : " + PlayerPrefs.GetInt("highscore_1") + "\n" +
            "Highscore_2 : " + PlayerPrefs.GetInt("highscore_2") + "\n" +
            "Highscore_3 : " + PlayerPrefs.GetInt("highscore_3") + "\n" +
            "Highscore_4 : " + PlayerPrefs.GetInt("highscore_4") + "\n" +
            "Highscore_5 : " + PlayerPrefs.GetInt("highscore_5") + "\n" +
            "Highscore_6 : " + PlayerPrefs.GetInt("highscore_6") + "\n" +
            "Highscore_7 : " + PlayerPrefs.GetInt("highscore_7") + "\n" +
            "Highscore_8 : " + PlayerPrefs.GetInt("highscore_8") + "\n" +
            "Highscore_9 : " + PlayerPrefs.GetInt("highscore_9") + "\n" );
        return score;
    }

    public void ReloudScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SetScore(int score)
    {
        highscore[0] = PlayerPrefs.GetInt("highscore_0");
        highscore[1] = PlayerPrefs.GetInt("highscore_1");
        highscore[2] = PlayerPrefs.GetInt("highscore_2");
        highscore[3] = PlayerPrefs.GetInt("highscore_3");
        highscore[4] = PlayerPrefs.GetInt("highscore_4");
        highscore[5] = PlayerPrefs.GetInt("highscore_5");
        highscore[6] = PlayerPrefs.GetInt("highscore_6");
        highscore[7] = PlayerPrefs.GetInt("highscore_7");
        highscore[8] = PlayerPrefs.GetInt("highscore_8");
        highscore[9] = PlayerPrefs.GetInt("highscore_9");
        highscore[10] = score;

        for (int i = 9; i >= 0; i--)
        {
            if (highscore[i] < highscore[i + 1])
            {
                int save = highscore[i + 1];
                highscore[i + 1] = highscore[i];
                highscore[i] = save;
                highscoreCounter--;
            }

            /*Debug.Log(
                highscore[0] + " " +
                highscore[1] + " " +
                highscore[2] + " " +
                highscore[3] + " " +
                highscore[4] + " " +
                highscore[5] + " " +
                highscore[6] + " " +
                highscore[7] + " " +
                highscore[8] + " " +
                highscore[9]);*/
        }
        PlayerPrefs.SetInt("highscore_0", highscore[0]);
        PlayerPrefs.SetInt("highscore_1", highscore[1]);
        PlayerPrefs.SetInt("highscore_2", highscore[2]);
        PlayerPrefs.SetInt("highscore_3", highscore[3]);
        PlayerPrefs.SetInt("highscore_4", highscore[4]);
        PlayerPrefs.SetInt("highscore_5", highscore[5]);
        PlayerPrefs.SetInt("highscore_6", highscore[6]);
        PlayerPrefs.SetInt("highscore_7", highscore[7]);
        PlayerPrefs.SetInt("highscore_8", highscore[8]);
        PlayerPrefs.SetInt("highscore_9", highscore[9]);

        Text first = HighscoreBoard.transform.GetChild(1).GetComponent<Text>();
        Text second = HighscoreBoard.transform.GetChild(2).GetComponent<Text>();
        Text third = HighscoreBoard.transform.GetChild(3).GetComponent<Text>();
        Text fourth = HighscoreBoard.transform.GetChild(4).GetComponent<Text>();
        Text fifth = HighscoreBoard.transform.GetChild(5).GetComponent<Text>();
        Text sixth = HighscoreBoard.transform.GetChild(6).GetComponent<Text>();
        Text seventh = HighscoreBoard.transform.GetChild(7).GetComponent<Text>();
        Text eighth = HighscoreBoard.transform.GetChild(8).GetComponent<Text>();
        Text ninth = HighscoreBoard.transform.GetChild(9).GetComponent<Text>();
        Text tenth = HighscoreBoard.transform.GetChild(10).GetComponent<Text>();

        first.text = "First : " + highscore[0];
        second.text = "Second : " + highscore[1];
        third.text = "Third : " + highscore[2];
        fourth.text = "4th Place : " + highscore[3];
        fifth.text = "5th Place : " + highscore[4];
        sixth.text = "6th Place : " + highscore[5];
        seventh.text = "7th Place : " + highscore[6];
        eighth.text = "8th Place : " + highscore[7];
        ninth.text = "9th Place : " + highscore[8];
        tenth.text = "10th Place : " + highscore[9];

        Text colour = HighscoreBoard.transform.GetChild(highscoreCounter).GetComponent<Text>();
        colour.color = new Color(255, 0, 0);

        HighscoreBoard.SetActive(true);

    }


}
