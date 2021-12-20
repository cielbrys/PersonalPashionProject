using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager managerScript;
    private sounds voiceOverScript;
    private float sizePowerUp = 2.9712f;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        managerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript.powerSoundFX = gameObject.GetComponent<AudioSource>();
        voiceOverScript = GameObject.Find("voiceOver").GetComponent<sounds>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        playerControllerScript.powerSoundFX.Play();
        playerControllerScript.powerUpName = gameObject.tag;
        playerControllerScript.hidePowerUpUi = false;
        if(gameObject.tag == "Cheese" )
        {
            LeanTween.scale(managerScript.uiCheese, new Vector3(0.185159f, 0.3711344f, 0), 0.5f);
            managerScript.uiPowerText.text = "spring boost";
            LeanTween.scale(managerScript.uiPowerupTextObj, new Vector3(0.1f, 0.1f, 0), 0.5f);

        } else if (gameObject.tag == "Ball")
        {
            LeanTween.scale(managerScript.uiBall, new Vector3(sizePowerUp, 2.9712f, 0), 0.5f);
            managerScript.uiPowerText.text = "levens boost";
            LeanTween.scale(managerScript.uiPowerupTextObj, new Vector3(0.1f, 0.1f, 0), 0.5f);
        }
        else if (gameObject.tag == "Serum" )
        {
            LeanTween.scale(managerScript.uiSerum, new Vector3(2.320336f, 3.187264f, 0), 0.5f);
            managerScript.uiPowerText.text = "Vlieg boost";
            LeanTween.scale(managerScript.uiPowerupTextObj, new Vector3(0.1f, 0.1f, 0), 0.5f);

        }

        if (managerScript.tutorial == 3)
        {
            playerControllerScript.tutorialPause = true;
            playerControllerScript.running = false;
            playerControllerScript.StopScore();
            if (voiceOverScript.played == 3)
            {
                voiceOverScript.voiceOver.PlayOneShot(voiceOverScript.voiceOvers[3], 1.0f);
                voiceOverScript.played++;
                foreach (GameObject uiTutorial in managerScript.uiTutorial3)
                {
                    LeanTween.scale(uiTutorial, new Vector3(1, 1, 1), 0.5f);
                }
            }
            LeanTween.scale(managerScript.uiPowerUp, new Vector3(0.2693726f, 0.2693726f, 0), 0.5f);
            LeanTween.moveLocal(managerScript.uiPowerUp, new Vector3(-315, 60, 0), 0.5f);
            LeanTween.moveLocal(managerScript.uiPowerupTextObj, new Vector3(-315, -30, 0), 0.5f);
            LeanTween.scale(managerScript.uiPowerupTextObj, new Vector3(0.15f, 0.15f, 0), 0.5f);



        }

        Destroy(gameObject);
    }



}

// -77 80