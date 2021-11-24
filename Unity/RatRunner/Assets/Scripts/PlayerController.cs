using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    public bool isLeft = false;
    public bool isRight = false;
    public bool isMiddle = true;
    public bool running = false;
    public float jumpForce = 20;

    private Rigidbody playerRb;
    private GameManager gameManagerScript;

    private Vector3 leftPos;
    private Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        leftPos = new Vector3(-8, 0, -8);
        rightPos = new Vector3(8, 0, -8);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !gameManagerScript.gameOver)
        {
            running = true;
            if (Input.GetKeyUp(KeyCode.LeftArrow) && !isLeft)
            {
                playerRb.position = playerRb.position + new Vector3(-8, 0, 0);

                isRight = false;

                if (playerRb.position.x < leftPos.x + 1 && playerRb.position.x > leftPos.x - 1)
                {
                    isMiddle = false;
                    isLeft = true;
                }
                else
                {
                    isMiddle = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) && !isRight)
            {
                playerRb.position = playerRb.position + new Vector3(8, 0, 0);
                isLeft = false;
                if (playerRb.position.x < rightPos.x + 1 && playerRb.position.x > rightPos.x - 1)
                {
                    isMiddle = false;
                    isRight = true;

                }
                else
                {
                    isMiddle = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("jumps");
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        } else
        {
            running = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game OVER");
            gameManagerScript.gameOver = true;
        }
    }

}
