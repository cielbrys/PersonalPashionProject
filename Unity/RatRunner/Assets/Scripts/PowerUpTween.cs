using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTween : MonoBehaviour

{
    private PlayerController playerControllerScript;
    private GameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        managerScript = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
    }
}
