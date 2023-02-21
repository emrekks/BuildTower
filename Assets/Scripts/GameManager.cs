using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static int ColorIndex;

    public static MovingCube FirstCube;

    public static ParticleSystem particalSystem;

    public static int Combo;

    public static int ScoreStatic;
    
    public float cameraSmoothTime;
    
    public Transform camera;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI startGameText;
  
    private CubeSpawner[] _spawners;
    
    private CubeSpawner _currentSpawner;

    private int _score;
    
    private int _spawnerIndex;

    private int _cameraIndex;

    private bool startClick = true;
   
    private Vector3 velocity;
    
    private Vector3 _targetCameraPos;



    private void Awake()
    {
        particalSystem =  GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
        
        FirstCube = GameObject.Find("FirstCube").GetComponent<MovingCube>();
        
        _spawners = FindObjectsOfType<CubeSpawner>();

        _targetCameraPos = camera.transform.position;

        ColorIndex = 0;

        Combo = 0;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startGameText.gameObject.SetActive(false);
            
            if (MovingCube.CurrentCube != null && !startClick)
            {
                MovingCube.CurrentCube.Stop();
            }

            _spawnerIndex = (_spawnerIndex == 0 ? 1 : 0);
          
            _currentSpawner = _spawners[_spawnerIndex];
                
            _currentSpawner.SpawnCube();

            if (_cameraIndex >= 10)
            {
                _targetCameraPos += new Vector3(0,0.1f, 0);
            }

            _cameraIndex++;

            startClick = false;
            
            Score();
        }

        if (camera.localPosition != _targetCameraPos)
        {
            camera.localPosition = Vector3.SmoothDamp(camera.localPosition, _targetCameraPos, ref velocity, cameraSmoothTime);
        }
    }

    private void Score()
    {
        if (!scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
        }

        scoreText.text = _score.ToString();

        if (Combo > 3)
        {
            _score += 3;
        }
        
        else if (Combo > 6)
        {
            _score += 6;
        }

        else
        {
            _score++;
        }

        ScoreStatic = _score;
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
