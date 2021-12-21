using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public float speed = 0;
    public bool gameOver;

    public GameObject uiBall;
    public GameObject uiCheese;
    public GameObject uiSerum;
    public GameObject uiPowerUp;

    public GameObject uiPowerupTextObj;
    public TextMeshProUGUI uiPowerText;


    public GameObject uiStart;
    public GameObject uiDead;
    public TextMeshProUGUI uiScore;
    private PlayerController playerControllerScript;
    public float scoreUpdate;

    public int tutorial = 0;

    public AudioSource SoundTrack;

    public GameObject[] uiTutorial1;
    public GameObject[] uiTutorial2;
    public GameObject[] uiTutorial3;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.init(800);
        gameOver = false;
        tutorial = 1;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreUpdate = 1f;
        SoundTrack = GameObject.Find("SoundTrack").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameStarting)
        {
            uiStart.gameObject.SetActive(false);
            if (!SoundTrack.isPlaying)
            {
                SoundTrack.Play();
            }
        }

        if (gameOver)
        {
            uiScore.text = Math.Round(playerControllerScript.score,1).ToString() + " meter";
            uiDead.gameObject.SetActive(true);
        }

        if(playerControllerScript.usePowerUp && playerControllerScript.powerUpName == "Serum")
        {
            scoreUpdate = 0.5f;
            speed = 60f;
        } else if (speed == 60f && scoreUpdate == 0.5f)
        {
            scoreUpdate = 1f;
            speed = 30f;
        }
 
    }
}
