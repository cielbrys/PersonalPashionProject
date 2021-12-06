using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] powerUpPrefabs;
    private float startDelay = 3;
    private float repeatRate = 5;
    private float repeatPowerRate = 25;
    private float startDelayPower = 20;
    private PlayerController playerControllerScript;
    public int[] spawnPos;
    private GameManager gameManagerScript;

    int randomPowerUp;
    int randomPowerUpSpawnPos;

    int randomObstacle;
    int randomSpawnPos;
    int randomSpawnPosZ;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnPowerUp", startDelayPower, repeatPowerRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void SpawnObstacle()
    {
        if (!gameManagerScript.gameOver && playerControllerScript.running && gameManagerScript.tutorial == 4)
        {
            randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            randomSpawnPos = Random.Range(0, spawnPos.Length);
            randomSpawnPosZ = Random.Range(75, 100);
            Instantiate(obstaclePrefabs[randomObstacle], new Vector3(spawnPos[randomSpawnPos], 0, randomSpawnPosZ), obstaclePrefabs[randomObstacle].transform.rotation);
            SpawnSecondObstacle();
        }
    }

    void SpawnSecondObstacle()
    {
        int randomObstacle2 = Random.Range(0, obstaclePrefabs.Length);
        int randomSpawnPos2 = Random.Range(0, spawnPos.Length);
        int randomSpawnPosZ2 = Random.Range(70, 100);
        if (randomObstacle2 != randomObstacle && randomSpawnPos2 != randomSpawnPos)
        {
            Instantiate(obstaclePrefabs[randomObstacle2], new Vector3(spawnPos[randomSpawnPos2], 0, randomSpawnPosZ2), obstaclePrefabs[randomObstacle2].transform.rotation);
        } else
        {
            SpawnSecondObstacle();
        }
    }

    void SpawnPowerUp()
    {
        if (gameManagerScript.gameOver == false && playerControllerScript.running == true && playerControllerScript.powerUpName == "" && gameManagerScript.tutorial == 4)
        {
            randomPowerUp = Random.Range(0, powerUpPrefabs.Length);
            randomPowerUpSpawnPos = Random.Range(0, spawnPos.Length);
            Instantiate(powerUpPrefabs[randomPowerUp], new Vector3(spawnPos[randomPowerUpSpawnPos], 2, 80), powerUpPrefabs[randomPowerUp].transform.rotation);
     
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
