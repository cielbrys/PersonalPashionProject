using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = gameManagerScript.speed;
        if (playerControllerScript.running == true)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }
}
