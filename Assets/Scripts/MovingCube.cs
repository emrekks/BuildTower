using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
   public static MovingCube CurrentCube { get; private set; }
   public static MovingCube LastCube { get; private set; }

   public Color[] color;
   private Color _firstColor;
   private bool _changeColor;
   private bool _backToFirstColor;
   public Renderer renderer;
 
   public MoveDirection MoveDirection { get; set; }

   private MaterialPropertyBlock _materialPropertyBlock;

   [SerializeField] private int moveSpeed = 2;
   [SerializeField] private float tolerance = 0.03f;

   public ParticleSystem comboParticleSystem;
   
   private int m_MoveSpeed;

   private void OnEnable()
   {
      comboParticleSystem = GameManager.particalSystem;
               
      m_MoveSpeed = moveSpeed;
      
      if (LastCube == null)
      {
         LastCube = GameManager.FirstCube;
      }

      CurrentCube = this;

      _firstColor = GetRandomColor();

      GameManager.IncreaseColorIndex(color.Length);

      transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);

      _materialPropertyBlock = new MaterialPropertyBlock();

      _materialPropertyBlock.SetColor("_Color", _firstColor);
      
      renderer.SetPropertyBlock(_materialPropertyBlock);
   }

   private Color GetRandomColor()
   {
      return color[GameManager.ColorIndex];
   }

   public void Stop()
   {
      m_MoveSpeed = 0;
      
      float distance = Distance();

      float lastCubeLocalScale = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;

      if (Mathf.Abs(distance) >= lastCubeLocalScale)
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
      
      if (fallingBlockSize < tolerance && fallingBlockSize > 0)
      {
         if (LastCube == GameManager.FirstCube)
         {
            CurrentCube.transform.position = new Vector3(LastCube.transform.position.x,LastCube.transform.position.y + 0.3f, LastCube.transform.position.z);
            
            CallParticleSystem();
         }
         
         else
         {
            CurrentCube.transform.position = new Vector3(LastCube.transform.position.x,LastCube.transform.position.y + 0.1f, LastCube.transform.position.z);
            
            CallParticleSystem();
         }
      }

      else
      {
         float newZPosition = LastCube.transform.position.z + (distance / 2);
        
         transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
         
         transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
     
         float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        
         float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

         SpawnDropPartOfCube(fallingBlockZPosition, fallingBlockSize);

         _changeColor = true;
      }
      
   }
   
   private void SplitCubeOnX(float distance, float direction)
   {
      float newXSize = LastCube.transform.localScale.x - Mathf.Abs(distance);
     
      float fallingBlockSize = transform.localScale.x - newXSize;

      if (fallingBlockSize < tolerance && fallingBlockSize > 0)
      {
         if (LastCube == GameManager.FirstCube)
         {
            CurrentCube.transform.position = new Vector3(LastCube.transform.position.x,LastCube.transform.position.y + 0.3f, LastCube.transform.position.z);
            
            CallParticleSystem();
         }
         
         else
         {
            CurrentCube.transform.position = new Vector3(LastCube.transform.position.x,LastCube.transform.position.y + 0.1f, LastCube.transform.position.z);

            CallParticleSystem();
         }
      }

      else
      {
         float newXPosition = LastCube.transform.position.x + (distance) / 2;
      
         transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
       
         transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
     
         float cubeEdge = transform.position.x + (newXSize / 2f * direction);
       
         float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

         SpawnDropPartOfCube(fallingBlockXPosition, fallingBlockSize);
         
         _changeColor = true;
      }
   }

   private void CallParticleSystem()
   {
      comboParticleSystem.transform.position = new Vector3(CurrentCube.transform.position.x, CurrentCube.transform.position.y, CurrentCube.transform.position.z);
      
      comboParticleSystem.transform.localScale = new Vector3(CurrentCube.transform.localScale.x / 2, CurrentCube.transform.localScale.z / 2, 0.5f);

      comboParticleSystem.gameObject.SetActive(true);
     
      comboParticleSystem.Play();
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
     
      cube.GetComponent<Renderer>().SetPropertyBlock(_materialPropertyBlock);

      Destroy(cube.gameObject, 1f);
   }

   private void Update()
   {
      if (MoveDirection == MoveDirection.Z)
      {
         transform.position += -transform.forward * Time.deltaTime * m_MoveSpeed;
         
         if (transform.position.z <= -2f)
         {
            GameOver();
         }
      }
      
      else
      {
         transform.position += transform.right * Time.deltaTime * m_MoveSpeed;
         
         if (transform.position.x >= 2f)
         {
            GameOver();
         }
      }

      if (_changeColor)
      {
         Color colour = Color.Lerp(_materialPropertyBlock.GetColor("_Color"), Color.white, 24 * Time.deltaTime);
         
         _materialPropertyBlock.SetColor("_Color", colour);
      
         renderer.SetPropertyBlock(_materialPropertyBlock);
         
         if (_materialPropertyBlock.GetColor("_Color") == Color.white)
         {
            _changeColor = false;
            _backToFirstColor = true;
         }
      }

      else if (!_changeColor)
      {
         Color colour = Color.Lerp(_materialPropertyBlock.GetColor("_Color"), _firstColor, 36 * Time.deltaTime);
         
         _materialPropertyBlock.SetColor("_Color", colour);
      
         renderer.SetPropertyBlock(_materialPropertyBlock);
      }
   }
}