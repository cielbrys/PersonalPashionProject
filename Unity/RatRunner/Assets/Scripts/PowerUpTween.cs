using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTween : MonoBehaviour

{
    private PlayerController playerControllerScript;
    private bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.running && !rotating)
        {
            rotating = true;
            LeanTween.rotateAroundLocal(gameObject, new Vector3(0, 30, 0), -720, 2f).setRepeat(-1);
        } else
        {
            rotating = false;
            LeanTween.cancel(gameObject);
        }
    }
}
