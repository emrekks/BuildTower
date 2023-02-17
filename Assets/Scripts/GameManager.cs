using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int ColorIndex;

    public static MovingCube FirstCube;
    
    public Transform camera;

    private Vector3 _targetCameraPos;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI startGameText;
  
    private CubeSpawner[] _spawners;
    
    private CubeSpawner _currentSpawner;
    
    private int _spawnerIndex;

    private int _score;
    
    private int _cameraIndex = 0;

    private bool startClick = true;
   
    private Vector3 velocity;
   
    public float cameraSmoothTime;
    


    private void Awake()
    {
        FirstCube = GameObject.Find("FirstCube").GetComponent<MovingCube>();
        
        _spawners = FindObjectsOfType<CubeSpawner>();

        _targetCameraPos = camera.transform.position;

        ColorIndex = 0;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            scoreText.gameObject.SetActive(true);
        
            startGameText.gameObject.SetActive(false);
            
            if (MovingCube.CurrentCube != null && !startClick)
            {
                MovingCube.CurrentCube.Stop();
            }

            scoreText.text = _score.ToString();

            _spawnerIndex = (_spawnerIndex == 0 ? 1 : 0);
          
            _currentSpawner = _spawners[_spawnerIndex];
                
            _currentSpawner.SpawnCube();

            if (_cameraIndex >= 10)
            {
                _targetCameraPos += new Vector3(0,0.1f, 0);
            }

            _cameraIndex++;
            
            _score++;

            startClick = false;
        }
        
        if (camera.localPosition != _targetCameraPos)
        {
            camera.localPosition = Vector3.SmoothDamp(camera.localPosition, _targetCameraPos, ref velocity, cameraSmoothTime);
        }
    }

    public static void IncreaseColorIndex(int i)
    {
        if (ColorIndex < i - 1)
        {
            ColorIndex++;
        }
      
        else
        {
            ColorIndex = 0;
        }
    }
}
