using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
   [SerializeField] private MovingCube cubePrefab;
  
   [SerializeField] private List<MovingCube> pooledCubes = new List<MovingCube>();
 
   [SerializeField] private int amountToPool;
   
   [HideInInspector] public int activeCubeCount;
  
   private int _backToPoolIndex;
  
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
         
         cube.gameObject.SetActive(false);
        
         pooledCubes.Add(cube);
      }
   }
   
   public MovingCube GetPooledObject()
   {
      for(int i = 0; i < amountToPool; i++)
      {
         if(!pooledCubes[i].gameObject.activeInHierarchy)
         {
            return pooledCubes[i];
         }
      }
      return null;
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
