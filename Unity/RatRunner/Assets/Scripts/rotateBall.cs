using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateBall : MonoBehaviour
{

    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.usePowerUp && playerControllerScript.powerUpName == "Ball")
        {
            LeanTween.rotateY(gameObject, 720, 10f).setRepeat(-1);
        }

    }
}
