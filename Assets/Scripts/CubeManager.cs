using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeManager : MonoBehaviour
{
    public MovingCube prefab;

    public GameObject firstCube;

    public MovingCube currentCube;

    public GameObject dropPartCube;

    private bool gameOver = false;

    private void Start()
    {
        Invoke("SpawnObject", 2f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentCube != null && !gameOver)
        {
            currentCube.clicked = true;
            
            CheckPosition();
        }


        
    }
    
    public void SpawnObject()
    {
        MovingCube newCube = Instantiate(prefab, transform.position, Quaternion.identity, transform);
       
        currentCube = newCube.GetComponent<MovingCube>();
    }
    
    
    private void CheckPosition()
    {
        float contact = currentCube.transform.position.x - firstCube.transform.position.x;

        if (Mathf.Abs(contact) >= firstCube.transform.localScale.x)
        {
            firstCube = null;
            currentCube = null;
            gameOver = true;
            Debug.Log("Game Finished");
        }

        float direction = contact > 0 ? 1f : -1f;
        
        float newSize = firstCube.transform.localScale.x - Math.Abs(contact);
        
        float fallingBlockSize = currentCube.transform.localScale.x - newSize;

        float newPosition = firstCube.transform.position.x + (contact / 2);
        
        currentCube.transform.localScale = new Vector3(newSize, currentCube.transform.localScale.y, currentCube.transform.localScale.z);
        
        currentCube.transform.position = new Vector3(newPosition, currentCube.transform.position.y, currentCube.transform.position.z);

        float firstCubeCorner = currentCube.transform.position.x + (newSize / 2f * direction);
        
        float fallingBlockPosition = firstCubeCorner + fallingBlockSize / 2 * direction;

        SpawnDropCube(fallingBlockPosition, fallingBlockSize);
        
        firstCube = currentCube.gameObject;

        currentCube = null;

        Invoke("SpawnNewCube", 2f);
    }

    private void SpawnDropCube(float fallingPartPosition, float fallingBlockSize)
    {
        GameObject droppedPart = Instantiate(dropPartCube, new Vector3(fallingPartPosition, currentCube.transform.position.y, currentCube.transform.position.z), Quaternion.identity, transform);
        
        droppedPart.transform.localScale = new Vector3(fallingBlockSize, droppedPart.transform.localScale.y, droppedPart.transform.localScale.z);
       
        Destroy(droppedPart, 1f);
    }

    private void SpawnNewCube()
    {
       var newObj = Instantiate(prefab, firstCube.transform.position, Quaternion.identity, transform);
       
       newObj.transform.localScale = firstCube.transform.localScale;
       
       currentCube = newObj;
    }
   
}
