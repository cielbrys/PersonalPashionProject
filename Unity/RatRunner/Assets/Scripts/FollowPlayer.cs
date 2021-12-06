using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerControllerScript.onGround)
        {
            transform.position = new Vector3(0, 5 + player.transform.position.y, -10);
        } else
        {
            transform.position = new Vector3(0, 5, -10);

        }
    }
}
