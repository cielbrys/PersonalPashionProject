using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLean : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(gameObject, 3, 1f).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
