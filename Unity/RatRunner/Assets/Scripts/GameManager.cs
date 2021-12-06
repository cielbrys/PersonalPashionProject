using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float speed = 30f;
    public bool gameOver;

    public GameObject uiBall;
    public GameObject uiCheese;
    public GameObject uiSerum;
    public GameObject uiPowerUp;

    public GameObject uiTutorial1;
    public GameObject uiTutorial2;
    public GameObject uiTutorial3;

    public GameObject uiStart;
    public GameObject uiDead;
    public TextMeshProUGUI uiScore;
    private PlayerController playerControllerScript;
    public float scoreUpdate;

    public int tutorial = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        tutorial = 1;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreUpdate = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.running)
        {
            uiStart.gameObject.SetActive(false);
        }

        if (gameOver)
        {
            uiScore.text = playerControllerScript.score.ToString() + " meter";
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
