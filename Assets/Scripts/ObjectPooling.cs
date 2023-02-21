using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
   [HideInInspector] public int activeCubeCount;
   
   [SerializeField] private MovingCube cubePrefab;
  
   [SerializeField] private List<MovingCube> pooledCubes = new List<MovingCube>();
 
   [SerializeField] private int amountToPool;

   private int _backToPoolIndex;

   private int _scoreSave = 25;
  
   #region Singleton
 
   public static ObjectPooling instance;

   private void Awake()
   {
      instance = this;
   }
   
   #endregion

   private void Start()
   {
      MovingCube cube;
      
      for(int i = 0; i < amountToPool; i++)
      {
         cube = Instantiate(cubePrefab);

         pooledCubes.Add(cube);
      }
   }

   public MovingCube GetPooledObject()
   {
      IncreaseSpeed();
      
      for(int i = 0; i < amountToPool; i++)
      {
         if(!pooledCubes[i].gameObject.activeInHierarchy)
         {
            return pooledCubes[i];
         }
      }
      
      return null;
   }

   public void IncreaseSpeed()
   {
      if (GameManager.ScoreStatic > _scoreSave)
      {
         _scoreSave += 25;
         
         foreach (var cubes in pooledCubes)
         {
            cubes.moveSpeed += 0.25f;
         }
      }
   }

   public void BackToPool()
   {
      if (activeCubeCount == pooledCubes.Count)
      {
         pooledCubes[_backToPoolIndex].gameObject.SetActive(false);

         activeCubeCount--;

         _backToPoolIndex++;

         if (_backToPoolIndex == pooledCubes.Count)
         {
            _backToPoolIndex = 0;
         }
      }
   }
}
