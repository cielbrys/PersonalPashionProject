using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    public bool running;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        running = playerControllerScript.running;
    }

    // Update is called once per frame
    void Update()
    {
        running = playerControllerScript.running;
        if (playerControllerScript.running == true)
        {
            transform.Translate(Vector3.back * Time.deltaTime * gameManagerScript.speed);
        }
    }
}
