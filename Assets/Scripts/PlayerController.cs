using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey("w"))
        {
            position.y += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            position.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            position.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            position.x -= speed * Time.deltaTime;
        }

        transform.position = position;
    }
}