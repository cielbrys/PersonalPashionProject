using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.running == true)
        {
            transform.Translate(Vector3.back * Time.deltaTime * (gameManagerScript.speed * 10));
        }
    }
}
