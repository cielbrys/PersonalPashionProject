using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( Input.GetKeyUp(KeyCode.Space))
        {
            transform.position = new Vector3(0, transform.position.y + player.transform.position.y, -18);
        } else
        {
            transform.position = new Vector3(0, 5, -18);

        }
    }
}
