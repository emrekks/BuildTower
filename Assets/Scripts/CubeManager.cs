using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeManager : MonoBehaviour
{
    public MovingCube prefab; //Prefab of cube

    public GameObject lastCube; //Last cube placed

    public GameObject dropPartCube; //Prefab of the falling piece from the shattered cube
    
    private MovingCube _currentCube; //Current cube moving

    private bool _gameOver = false; //Check gameover

    private bool firstTimeStartGame = false; //Using for spawn condition

    private void Start()
    {
        Invoke("SpawnNewCube", 2f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _currentCube != null && !_gameOver)
        {
            _currentCube.clicked = true; //Stop movement of cube
            
            CheckPosition();
        }      
    }

    private void CheckPosition()
    {
        float contact = _currentCube.transform.position.x - lastCube.transform.position.x;

        if (Mathf.Abs(contact) >= lastCube.transform.localScale.x) GameFinished();
        
        float direction = contact > 0 ? 1f : -1f;
        
        float newSize = lastCube.transform.localScale.x - Math.Abs(contact);
        
        float fallingBlockSize = _currentCube.transform.localScale.x - newSize;

        float newPosition = lastCube.transform.position.x + (contact / 2);

        ChangeLocalScale(_currentCube.transform, newSize);

        ChangePosition(_currentCube.transform, newPosition);

        float lastCubeCorner = _currentCube.transform.position.x + (newSize / 2f * direction); //the edge of the last cube finds its position to place the falling cube
        
        float fallingBlockPosition = lastCubeCorner + fallingBlockSize / 2 * direction; //places the falling cube next to the current cube

        SpawnDropCube(fallingBlockPosition, fallingBlockSize);
        
        lastCube = _currentCube.gameObject;

        _currentCube = null;

        Invoke("SpawnNewCube", 2f);
    }

    private void ChangeLocalScale(Transform trans, float size)
    {
        trans.localScale = new Vector3(size, transform.localScale.y, trans.localScale.z);
    }
    
    private void ChangePosition(Transform trans, float position)
    {
        trans.position = new Vector3(position, transform.position.y, trans.position.z);
    }
    private void SpawnDropCube(float fallingPartPosition, float fallingBlockSize)
    {
        GameObject droppedPart = Instantiate(dropPartCube, new Vector3(fallingPartPosition, _currentCube.transform.position.y, _currentCube.transform.position.z), Quaternion.identity, transform);
        
        droppedPart.transform.localScale = new Vector3(fallingBlockSize, droppedPart.transform.localScale.y, droppedPart.transform.localScale.z);
       
        Destroy(droppedPart, 1f);
    }

    private void SpawnNewCube()
    {
       var newObj = Instantiate(prefab, transform.position, Quaternion.identity, transform);

       if (firstTimeStartGame)
       {
           newObj.transform.localScale = lastCube.transform.localScale;
       }

       _currentCube = newObj;
       
       firstTimeStartGame = true;
    }

    private void GameFinished()
    {
        lastCube = null;
        _currentCube = null;
        _gameOver = true;
        Debug.Log("Game Finished");
    }
   
}
