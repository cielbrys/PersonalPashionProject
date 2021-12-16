using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounds : MonoBehaviour
{

    public AudioClip[] voiceOvers;
    public AudioSource voiceOver;
    private GameManager gameManagerScript;
    private PlayerController playerControllerScript;
    public int played = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        voiceOver = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManagerScript.tutorial == 1 && played == 0 && playerControllerScript.gameStarting)
        {
            voiceOver.PlayOneShot(voiceOvers[0], 1.0f);
            played++;
        }
        else if (gameManagerScript.tutorial == 4 && playerControllerScript.gameStarting && played == 4)
        {
            voiceOver.PlayOneShot(voiceOvers[4], 1.0f);
            played++;

        }

    }
}
