using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPositionTut1 : MonoBehaviour
{
    private GameManager gameManagerScript;
    private PlayerController playerScript;
    public sounds voiceOverScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        voiceOverScript = GameObject.Find("voiceOver").GetComponent<sounds>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.z <= 25 && gameManagerScript.tutorial == 1)
        {
            playerScript.tutorialPause = true;
            playerScript.running = false;
            playerScript.StopScore();
            if (voiceOverScript.played == 1)
            {
                voiceOverScript.voiceOver.PlayOneShot(voiceOverScript.voiceOvers[1], 1.0f);
                voiceOverScript.played++;
                foreach (GameObject uiTutorial in gameManagerScript.uiTutorial1)
                {
                    LeanTween.scale(uiTutorial, new Vector3(1, 1, 1), 0.5f);
                }
            }
        }
    }
}
