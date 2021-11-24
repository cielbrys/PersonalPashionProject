using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float startDelay = 3;
    private float repeatRate = 2;
    private PlayerController playerControllerScript;
    private float backBound = 11;
    public int[] spawnPos;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void SpawnObstacle()
    {
        if (gameManagerScript.gameOver == false && playerControllerScript.running == true)
        {
            int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            int randomSpawnPos = Random.Range(0, spawnPos.Length);
            Instantiate(obstaclePrefabs[randomObstacle], new Vector3(spawnPos[randomSpawnPos], 0, 40), obstaclePrefabs[randomObstacle].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
