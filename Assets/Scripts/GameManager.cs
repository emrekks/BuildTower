using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CubeSpawner[] _spawners;
    private int _spawnerIndex;
    private CubeSpawner _currentSpawner;
    public Camera camera;
    private int cameraIndex = 0;

    private void Awake()
    {
        _spawners = FindObjectsOfType<CubeSpawner>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MovingCube.CurrentCube != null)
            {
                MovingCube.CurrentCube.Stop();
            }
            
            _spawnerIndex = (_spawnerIndex == 0 ? 1 : 0);
            _currentSpawner = _spawners[_spawnerIndex];
                
            _currentSpawner.SpawnCube();

            if (cameraIndex >= 5)
            {
                camera.transform.position = new Vector3(camera.transform.position.x,
                    camera.transform.position.y + 0.1f, camera.transform.position.z);
            }

            cameraIndex++;
        }
    }
}
