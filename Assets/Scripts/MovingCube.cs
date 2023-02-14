using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class MovingCube : MonoBehaviour
{
    public Rigidbody rb;

    public MeshRenderer renderer;
    
    [Tooltip("Speed of cube")]public float speed = 5f; 
    
    [Tooltip("Changing direction between x and z")]public bool directionChanger; 
    
    [Tooltip("Height of cube")]public float y;
    
    [Tooltip("Determines the distance between two destinations")]public float range;

    [Tooltip("Stops the update function when left click is pressed")]public bool clicked = false;

    private void Start()
    {
        rb.useGravity = false;

        renderer.material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }

    private void Update()
    {
        if (clicked)
        {
            rb.useGravity = true;
            return;
        }
        
        Vector3 movement = directionChanger ? new Vector3(Mathf.Cos(Time.time * speed) * range, y, 0) : new Vector3(0, y, Mathf.Cos(Time.time * speed) * range);
        
        transform.position = movement;
    }
}
