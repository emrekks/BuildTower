using UnityEngine;


public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private MovingCube cubePrefab;
    
    [SerializeField] private MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = ObjectPooling.instance.GetPooledObject();

        ObjectPooling.instance.activeCubeCount++;

        cube.gameObject.SetActive(true);
        
        ObjectPooling.instance.BackToPool();
        
        if (MovingCube.LastCube != null && MovingCube.LastCube != GameManager.FirstCube)
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;
            
            cube.transform.position = new Vector3(x, MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y, z);
        }
        
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
      
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }

}

public enum MoveDirection
{
    X,
    Z
}
