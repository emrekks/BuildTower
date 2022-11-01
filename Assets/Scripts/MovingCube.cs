using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovingCube : MonoBehaviour
{
    public Rigidbody rb;
    
    public float speed = 5f;
    
    public bool directionChanger;
    
    public float y;
    
    public float range;

    public bool clicked = false;

    private void Update()
    {
        if (clicked)
        {
            rb.useGravity = true;
            return;
        }
        Vector3 movement = directionChanger ? new Vector3(Mathf.Cos(Time.time * speed) * range, y, 0) : new Vector3(0, y, Mathf.Cos(Time.time * speed) * range);
        transform.position = movement;
        rb.useGravity = false;
    }
}
