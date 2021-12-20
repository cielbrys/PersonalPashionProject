using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{    public bool isLeft = false;
    public bool isRight = false;
    public bool isMiddle = true;
    public bool running = false;
    public bool changing = false;
    public bool onGround = true;

    public int score;
    public TextMeshProUGUI scoreText;
    private bool scoreUpdating = false;

    public bool usePowerUp = false;
    public string powerUpName = "";
    public GameObject ballIndicator;
    public GameObject flyIndicator;
    public int powerUpTimer = 15;
    public TextMeshProUGUI uiTimer;
    public GameObject uiTimerObj;
    public AudioSource powerSoundFX;
    private AudioSource powerTimerSoundFX;
    private AudioSource powerTransformSoundFX;


    public int jumpForce = 150;
    public float turnForce = 20;

    private Rigidbody playerRb;
    private GameManager gameManagerScript;
    private ArduinoController arduinoControllerScript;
    private SpawnManager spawnManagerScript;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private int prevLeft;
    private int prevRight;
    private int prevUp;
    private int prevDown;
    private int prevMid;

    private string[] dataSplit;

    private Animator playerAnim;

    public ParticleSystem powerUpFX;
    public ParticleSystem deadFX;
    public ParticleSystem flyPowerUp;
    public ParticleSystem cheesePowerUp;

    public bool hidePowerUpUi = true;

    public bool gameStarting = false;
    public bool tutorialPause = false;

    public GameObject[] distanceMeterUi;
    public GameObject distanceUi;
    private bool startedShowing = false;

    private AudioSource deadSoundFx;

    private AudioSource profSoundFx;

    public GameObject profSoundObj;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        arduinoControllerScript = GameObject.Find("Arduino").GetComponent<ArduinoController>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        powerTimerSoundFX = uiTimerObj.GetComponent<AudioSource>();
        powerTransformSoundFX = powerUpFX.GetComponent<AudioSource>();
        deadSoundFx = gameObject.GetComponent<AudioSource>();
        profSoundFx = profSoundObj.GetComponent<AudioSource>();

        leftPos = new Vector3(-8, 0, 0);
        rightPos = new Vector3(8, 0, 0);

        prevMid = 0;
        running = false;

        score = 0;

        playerAnim = GetComponent<Animator>();

    }

    void restartGame()
    {;
        arduinoControllerScript.stream.Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    // Update is called once per frame
    void Update()
    {

        //check for arduino data
        if(arduinoControllerScript.data !=null)
        {
           dataSplit = arduinoControllerScript.data.Split(',');
        } else
        {
            dataSplit = null ;
            dataSplit = new string[]{ "0", "0", "0", "0", "0", "0" };
        }

        //start Game

        if(float.Parse(dataSplit[5]) == 1 && !gameManagerScript.gameOver && !running && prevMid == 0 && gameManagerScript.tutorial ==1)
        {
            gameStarting = true;
        }

        //restart Game
        if (float.Parse(dataSplit[5]) == 1 && gameManagerScript.gameOver && prevMid == 0)
        {
            restartGame();

        }

        //check if running
        if (gameStarting && float.Parse(dataSplit[0]) > 0.38 && !tutorialPause && !gameManagerScript.gameOver)
        {
            //gameManagerScript.speed = float.Parse(dataSplit[0]);
            running = true;
            if (!scoreUpdating)
            {
                StartUpateScore();
            }
            if (gameManagerScript.tutorial >= 4)
            {
                hideProf();
            }

        }
        else if(gameStarting && float.Parse(dataSplit[0]) <= 0.38 && !tutorialPause && !gameManagerScript.gameOver)
        {
            if (usePowerUp && powerUpName == "Serum")
            {
                running = true;
            }
            else if(!onGround)
            {
                running = true;

            } else
            {
                running = false;
                StopScore();
                if (gameManagerScript.tutorial >= 4)
                {
                    showProf();
                }
            }
        }

        //when running
        if (running && !gameManagerScript.gameOver && gameManagerScript.tutorial >= 2)
        {
            playerAnim.SetTrigger("running_trig");

            if (float.Parse(dataSplit[1]) == 1 && prevLeft == 0 && !isLeft && !changing)
            {
                ChangeLane(300);
                
            }
            else if (float.Parse(dataSplit[2]) == 1 && prevRight == 0 && !isRight && !changing)
            {
                ChangeLane(-300);
            
            }
            else if (float.Parse(dataSplit[3]) == 1 && prevUp == 0 && onGround)
            {
                Jump();
            }
            else if (float.Parse(dataSplit[4]) == 1 && prevDown == 0)
            {
                Down();
            }

            if (float.Parse(dataSplit[5]) == 1 && prevMid == 0 && powerUpName != "" && !usePowerUp)
            {
                ActivatePowerUp();
             }



            if (usePowerUp)
            {
                if (powerUpFX.time >= 0.50)
                {
                    if (powerUpName == "Ball")
                    {
                        ballIndicator.gameObject.SetActive(true);
                    }
                    else if (powerUpName == "Serum")
                    {
                        flyIndicator.gameObject.SetActive(true);
                    }
                }

                if (powerUpName == "Serum")
                {
                    flyPowerUp.Play();
                    playerAnim.speed = 0;
                    onGround = false;
                    running = true;
                    playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                    if (transform.position.y >= 10)
                    {
                        playerRb.velocity = Vector3.zero;
                        gameManagerScript.speed = 4;
                    }
                } else if (powerUpName == "Cheese")
                {
                    cheesePowerUp.Play();
                }
            }



            //set previous button data, prevent changes when button is pushed in all the time
            prevDown = int.Parse(dataSplit[4]);
            prevUp = int.Parse(dataSplit[3]);
            prevRight = int.Parse(dataSplit[2]);
            prevLeft = int.Parse(dataSplit[1]);
            prevMid = int.Parse(dataSplit[5]);

        }

        //tutorial
           if(gameManagerScript.tutorial < 4)
        {
            PlayTutorial();
        }


        //check player pos

        if (playerRb.position.x < leftPos.x + 0.8 && playerRb.position.x > leftPos.x - 0.8 && !isLeft)
        {
            isMiddle = false;
            isLeft = true;
            setPlayerInLane(-8);
        }
        else if (playerRb.position.x < rightPos.x + 0.8 && playerRb.position.x > rightPos.x - 0.8 && !isRight)
        {
            isMiddle = false;
            isRight = true;
            setPlayerInLane(8);

        } else if(playerRb.position.x < 0 + 0.8 && playerRb.position.x > 0 - 0.8 && !isMiddle)
        {
            isMiddle = true;
            isRight = false;
            isLeft = false;
            setPlayerInLane(0);
        }

    }

    void countDownPowerUp()
    {
            uiTimer.text = powerUpTimer.ToString();
            powerUpTimer--;
    }

    private void setPlayerInLane(int xPos)
    {
        playerRb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        playerRb.useGravity = true;
        changing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (usePowerUp && powerUpName == "Ball")
            {
                changing = false;
                Destroy(collision.gameObject);

            } else if(!usePowerUp && powerUpName != "Serum")
            {
                playerRb.AddForce (Vector3.zero, ForceMode.Impulse);
                deadFX.Play();
                deadSoundFx.Play();
                playerRb.useGravity = true;
                running = false;
                gameManagerScript.gameOver = true;
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            playerAnim.speed = 1;
            changing = false;
        }
    }

    IEnumerator stopDown()
    {
        yield return new WaitForSeconds(1.5f);
        LeanTween.scaleY(gameObject, 1f, 0.2f);
    }

    void showProf()
    {
        if (!startedShowing)
        {
            profSoundFx.Play();
            foreach (GameObject uiMeter in distanceMeterUi)
            {
                LeanTween.cancel(uiMeter);
            }
            LeanTween.scale(distanceMeterUi[0], new Vector3(0f, 0f, 0), 8f).setOnComplete(GameOver);
            LeanTween.moveLocalX(distanceMeterUi[1], 0, 8f);
            LeanTween.moveLocalX(distanceMeterUi[2], 0, 8f);
            LeanTween.moveLocalZ(profSoundObj, 0, 8f);
            startedShowing = true;
        }
    }

    void hideProf()
    {
        if (startedShowing)
        {
            foreach (GameObject uiMeter in distanceMeterUi)
            {
                LeanTween.cancel(uiMeter);
            }
            LeanTween.scale(distanceMeterUi[0], new Vector3(0.09619795f, 0.09619795f, 0), 5f);
            LeanTween.moveLocalX(distanceMeterUi[1], 140, 5f);
            LeanTween.moveLocalX(distanceMeterUi[2], -140, 5f);
            LeanTween.moveLocalZ(profSoundObj, -65, 5f).setOnComplete(StopSoundPlay);
            startedShowing = false;
        }
    }

    void StopSoundPlay()
    {
        profSoundFx.Stop();
    }

    void GameOver()
    {
        profSoundFx.Stop();
        gameManagerScript.gameOver = true;
    }


    IEnumerator StopPowerUp()
    {
        yield return new WaitForSeconds(15);
        if(powerUpName == "Serum")
        {
            StopScore();
            gameManagerScript.scoreUpdate = 1f;
            StartUpateScore();
            gameManagerScript.speed = 2;
            playerRb.useGravity = true;
            flyPowerUp.Stop();
            flyIndicator.gameObject.SetActive(false);
            LeanTween.scale(gameManagerScript.uiSerum, new Vector3(0, 0, 0), 0.5f);
        } else if (powerUpName == "Ball")
        {
            LeanTween.scale(gameManagerScript.uiBall, new Vector3(0, 0, 0), 0.5f);
            ballIndicator.gameObject.SetActive(false);
        } else
        {
            cheesePowerUp.Stop();
            LeanTween.scale(gameManagerScript.uiCheese, new Vector3(0, 0, 0), 0.5f);
        }
        powerUpName = "";
        usePowerUp = false;
        CancelInvoke("countDownPowerUp");
        powerTimerSoundFX.Stop();
        powerUpTimer = 15;
        LeanTween.scale(uiTimerObj, new Vector3(0f, 0f, 0), 0.5f);
        LeanTween.cancel(gameManagerScript.uiPowerUp, true);
        LeanTween.scale(gameManagerScript.uiPowerUp, new Vector3(0.1368624f, 0.1368624f, 0), 0.5f);
        LeanTween.scale(gameManagerScript.uiPowerupTextObj, new Vector3(0f, 0f, 0), 0.5f);
    }

    void updateScore()
    {
        if (!gameManagerScript.gameOver)
        {
            score++;
            scoreText.text = score.ToString();
        }
    }

    void ChangeLane (int direction)
    {
        changing = true;
        playerRb.AddForce(Vector3.left * direction, ForceMode.Impulse);
    }

    void Jump (){
        if (usePowerUp && powerUpName == "Cheese")
        {
            playerRb.AddForce(Vector3.up * 300, ForceMode.Impulse);
        }
        else
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        onGround = false;
        changing = true;
        playerAnim.speed = 0;
    }

    void Down()
    {
        LeanTween.scaleY(gameObject, 0.8f, 0.2f);
        StartCoroutine(stopDown());
    }

    void ActivatePowerUp()
    {
        LeanTween.scale(uiTimerObj, new Vector3(4.6f, 4.6f, 0), 0.5f);
        InvokeRepeating("countDownPowerUp", 0, 1f);
        LeanTween.scale(gameManagerScript.uiPowerUp, new Vector3(0.1848601f, 0.1848601f, 0), 0.5f).setLoopPingPong();
        powerTimerSoundFX.Play();
        usePowerUp = true;
        powerUpFX.Play();
        powerTransformSoundFX.Play();
        StartCoroutine(StopPowerUp());
        if(powerUpName == "Serum")
        {
            StopScore();
            gameManagerScript.scoreUpdate = 0.5f;
            StartUpateScore();
        }
    }

    void PlayTutorial()
    {
        if (running)
        {
            playerAnim.SetTrigger("running_trig");
        }
        if (!running)
        {
            if (gameManagerScript.tutorial == 1)
            {
                if (float.Parse(dataSplit[1]) == 1 && prevLeft == 0 && !isLeft && !changing)
                {
                    ChangeLane(300);
                    gameManagerScript.tutorial = 2;
                    running = true;
                    tutorialPause = false;
                    StartUpateScore();
                    foreach (GameObject uiTutorial in gameManagerScript.uiTutorial1)
                    {
                        LeanTween.scale(uiTutorial, new Vector3(0, 0, 1), 0.5f).setDestroyOnComplete(true);
                    }


                }
                else if (float.Parse(dataSplit[2]) == 1 && prevRight == 0 && !isRight && !changing)
                {
                    ChangeLane(-300);
                    gameManagerScript.tutorial = 2;
                    running = true;
                    tutorialPause = false;
                    StartUpateScore();
                    foreach (GameObject uiTutorial in gameManagerScript.uiTutorial1)
                    {
                        LeanTween.scale(uiTutorial, new Vector3(0, 0, 1), 0.5f).setDestroyOnComplete(true);
                    }

                }
            }
            else if (gameManagerScript.tutorial == 2)
            {
                if (float.Parse(dataSplit[3]) == 1 && prevUp == 0 && onGround)
                {
                    gameManagerScript.tutorial = 3;
                    running = true;
                    tutorialPause = false;
                    StartUpateScore();
                    foreach (GameObject uiTutorial in gameManagerScript.uiTutorial2)
                    {
                        LeanTween.scale(uiTutorial, new Vector3(0, 0, 1), 0.5f).setDestroyOnComplete(true);
                    }
                    Jump();
                }
                else if (float.Parse(dataSplit[4]) == 1 && prevDown == 0)
                {
                    gameManagerScript.tutorial = 3;
                    running = true;
                    tutorialPause = false;
                    StartUpateScore();
                    foreach (GameObject uiTutorial in gameManagerScript.uiTutorial2)
                    {
                        LeanTween.scale(uiTutorial, new Vector3(0, 0, 1), 0.5f).setDestroyOnComplete(true);
                    }
                    Down();
                }
            }
            else if (gameManagerScript.tutorial == 3)
            {
                if (float.Parse(dataSplit[5]) == 1 && prevMid == 0 && powerUpName != "" && !usePowerUp)
                {
                    foreach (GameObject uiTutorial in gameManagerScript.uiTutorial3)
                    {
                        LeanTween.scale(uiTutorial, new Vector3(0, 0, 1), 0.5f).setDestroyOnComplete(true);
                    }
                    LeanTween.scale(gameManagerScript.uiPowerUp, new Vector3(0.1368624f, 0.1368624f, 0), 0.5f);
                    LeanTween.scale(gameManagerScript.uiPowerupTextObj, new Vector3(0.1f, 0.1f, 0), 0.5f);
                    LeanTween.moveLocal(gameManagerScript.uiPowerUp, new Vector3(-347, 85, 0), 0.5f);
                    LeanTween.moveLocal(gameManagerScript.uiPowerupTextObj, new Vector3(-347, 40, 0), 0.5f);
                    gameManagerScript.tutorial = 4;
                    running = true;
                    tutorialPause = false;
                    StartUpateScore();
                    ActivatePowerUp();
                    LeanTween.scale(distanceUi, new Vector3(1, 1, 0), 0.5f);
                }
            }
        }
    }

    public void StopScore()
    {

        CancelInvoke("updateScore");
        scoreUpdating = false;

    }

    void StartUpateScore()
    {
            InvokeRepeating("updateScore", 0, gameManagerScript.scoreUpdate);
            scoreUpdating = true;

    }

}
