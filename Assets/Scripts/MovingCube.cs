using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class MovingCube : MonoBehaviour
{
   public static MovingCube CurrentCube { get; private set; }
   public static MovingCube LastCube { get; private set; }
   
   public MoveDirection MoveDirection { get; set; }

   [SerializeField] private float moveSpeed = 2f;

   private void OnEnable()
   {
      if (LastCube == null)
      {
         LastCube = GameObject.Find("FirstCube").GetComponent<MovingCube>();
      }

      CurrentCube = this;

      GetComponent<Renderer>().material.color = GetRandomColor();
         
      transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y,
         LastCube.transform.localScale.z);
   }

   private Color GetRandomColor()
   {
      return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f),
         UnityEngine.Random.Range(0, 1f));
   }

   public void Stop()
   {
      moveSpeed = 0;
      float distance = Distance();

      float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;

      if (Mathf.Abs(distance) >= max)
      {
         GameOver();
      }
    
      float direction = distance > 0 ? 1f : -1f;
   
      if (MoveDirection == MoveDirection.Z)
      {
         SplitCubeOnZ(distance, direction);
      }

      else
      {
         SplitCubeOnX(distance, direction);
      }

      LastCube = this;
   }

   public void GameOver()
   {
      LastCube = null;
      CurrentCube = null;
      SceneManager.LoadScene(0);
   }
   
   private float Distance()
   {
      if (MoveDirection == MoveDirection.Z)
      {
         return transform.position.z - LastCube.transform.position.z;
      }
      else
      {
         return transform.position.x - LastCube.transform.position.x;
      }
   }

   private void SplitCubeOnZ(float distance, float direction)
   {
      float newZSize = LastCube.transform.localScale.z - Mathf.Abs(distance);
      float fallingBlockSize = transform.localScale.z - newZSize;
      
      float newZPosition = LastCube.transform.position.z + (distance / 2);
      transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
      transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
     
      float cubeEdge = transform.position.z + (newZSize / 2f * direction);
      float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

      SpawnDropPartOfCube(fallingBlockZPosition, fallingBlockSize);
   }
   
   private void SplitCubeOnX(float distance, float direction)
   {
      float newXSize = LastCube.transform.localScale.x - Mathf.Abs(distance);
      float fallingBlockSize = transform.localScale.x - newXSize;
      float newXPosition = LastCube.transform.position.x + (distance) / 2;
      transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
      transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
      float cubeEdge = transform.position.x + (newXSize / 2f * direction);
      float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

      SpawnDropPartOfCube(fallingBlockXPosition, fallingBlockSize);
   }

   private void SpawnDropPartOfCube(float fallingBlockZPosition, float fallingBlockSize)
   {
      var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

      if (MoveDirection == MoveDirection.Z)
      {
         cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
         cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
      }

      else
      {
         cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
         cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
      }
     
      cube.AddComponent<Rigidbody>();
      cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
      Destroy(cube.gameObject, 1f);
   }

   private void Update()
   {
      if (MoveDirection == MoveDirection.Z)
      {
         transform.position += -transform.forward * Time.deltaTime * moveSpeed;
         
         if (transform.position.z <= -7.5f)
         {
            GameOver();
         }
      }
      else
      {
         transform.position += transform.right * Time.deltaTime * moveSpeed;
         
         if (transform.position.x >= 6.65f)
         {
            GameOver();
         }
      }
   }
}