using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPositionTut2 : MonoBehaviour
{
    private GameManager gameManagerScript;
    private PlayerController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.z <= 15 && gameManagerScript.tutorial == 2)
        {
            playerScript.running = false;
            gameManagerScript.uiTutorial2.SetActive(true);

        }
    }
}
