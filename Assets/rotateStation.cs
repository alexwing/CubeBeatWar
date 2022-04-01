using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateStation : MonoBehaviour
{
    // Start is called before the first frame update
    public float velocity = 0.01f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rotate z
        transform.Rotate(0, 0, velocity);
    }
}
