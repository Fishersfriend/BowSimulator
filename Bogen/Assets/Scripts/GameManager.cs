using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject[] level;
    int currentLevel = 0;
    public bool gameActive = false;

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

    private bool gameOver = false;


    void Start()
    {
        NextLevel(0);

    }
    private void Awake() {
        zombiesToKill = new List<Zombie>();
        zombies = new List<Zombie>();
    }

    private void LateUpdate() {
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
                else GameOver("You won! Congratulations! :]");
            }

            if (player.health <= 0 && !gameOver) GameOver("You're dead. :[");
        }

    }

    private IEnumerator SpawnWave(Wave wave) {
        spawning = true;

        ui.Message = "Level " + (waveId + 1);
        ui.ShowMessage();
        yield return new WaitForSeconds(wavePause);
        ui.HideMessage();

        foreach (Zombie zombie in zombies) Destroy(zombie.gameObject);
        zombies = new List<Zombie>();

        int count = 0;
        while (count < wave.enemyCount) {
            zombiesToKill.Add(SpawnZombie());
            count++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        spawning = false;
    }

    private Zombie SpawnZombie() {
        int id = Random.Range(0, fence.wallCount);

        float step = 2 * Mathf.PI / fence.wallCount;
        float angle = step * id + Random.Range(0, step);
        Zombie newZombie = Instantiate<Zombie>(zombiePrefab, transform);
        newZombie.transform.localPosition = (new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle))) * spawnRadius;
        newZombie.transform.localEulerAngles = Vector3.up * (angle * Mathf.Rad2Deg + 180);
        FenceWall wall = fence.Walls[id].GetComponent<FenceWall>();
        if(wall.Alive) newZombie.targets.Add(wall);
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

    private void GameOver(string msg) {
        bow.gameObject.SetActive(false);
        ui.Message = msg;
        ui.ShowMessage();
        gameOver = true;
    }
}
