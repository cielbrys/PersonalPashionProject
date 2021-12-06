using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private GameManager managerScript;

    private float sizePowerUp = 0.4f;
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

    void OnTriggerEnter(Collider other)
    {
        playerControllerScript.powerUpName = gameObject.tag;
        playerControllerScript.hidePowerUpUi = false;
        if(gameObject.tag == "Cheese" )
        {
            LeanTween.scale(managerScript.uiCheese, new Vector3(sizePowerUp, 0.2203206f, 0), 0.5f);
        } else if (gameObject.tag == "Ball")
        {
            LeanTween.scale(managerScript.uiBall, new Vector3(sizePowerUp, sizePowerUp, 0), 0.5f);
        }
        else if (gameObject.tag == "Serum" )
        {
            LeanTween.scale(managerScript.uiSerum, new Vector3(0.2708255f, 0.3746946f, 0), 0.5f);
        }
        Destroy(gameObject);

        if(managerScript.tutorial == 3)
        {
            playerControllerScript.running = false;
            playerControllerScript.StopScore();
            managerScript.uiTutorial3.SetActive(true);
            LeanTween.scale(managerScript.uiPowerUp, new Vector3(1f, 1f, 0), 0.5f);
            LeanTween.moveLocal(managerScript.uiPowerUp, new Vector3(-250, 100, 0), 0.5f);

        }
    }



}
