using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolMoveAndControlScript : MonoBehaviour
{
    [Header("Pistol Movement")]
    public float pistolForwardSpeed;
    public float pistolHorizontalSpeed; 
    public float pistolXAxisMovementAmount;
    private float pistolNewXPosition;
    private float mouseXPosition;
    private float pistolXAxisMovePosition;

    [Header("Level")]
    public int levelNumber;

    [Header("Limits")]
    public Vector2 pistolXAxisLimits;

    [Header("Player")]
    public bool isPlayerAlive;
    public bool isPlayerTouchedScreen;
    public bool isLevelCompleted;

    private Rigidbody pistolRigidbody;

    PistolShootScript pistolShootScript;
    CanvasMenuScript canvasMenuScript;
    CameraFollowScript cameraFollowScript;

    private void Awake()
    {
        GetPistolRigidbody();
        PlayerAlive(true);
        TouchedScreen(false);
        LoadLevelNumber();
    }
    private void GetPistolRigidbody()
    {
        pistolRigidbody = GetComponent<Rigidbody>();
    }
    private void PlayerAlive(bool isAlive)
    {
        isPlayerAlive = isAlive;
    }
    private void TouchedScreen(bool isScreenTouched)
    {
        isPlayerTouchedScreen = isScreenTouched;
    }
    private void LoadLevelNumber()
    {
        levelNumber = PlayerPrefs.GetInt("LevelNumber");
        if (levelNumber < 1)
            levelNumber = 1;
    }



    // Start is called before the first frame update
    void Start()
    {
        GetOtherNeededScripts();
        WriteScreenBottom("Tap to Play");
    }
    private void GetOtherNeededScripts()
    {
        GetPistolShootScript();
        GetCanvasMenuScript();
        GetCameraFollowScript();
    }
    private void GetPistolShootScript()
    {
        pistolShootScript = GetComponent<PistolShootScript>();
    }
    private void GetCanvasMenuScript()
    {
        canvasMenuScript = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<CanvasMenuScript>();
    }
    private void GetCameraFollowScript()
    {
        cameraFollowScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowScript>();
    }
    private void WriteScreenBottom(string newString)
    {
        GetBottomText().GetComponent<Text>().text = newString;
    }
    private static Transform GetBottomText()
    {
        return GameObject.FindGameObjectWithTag("PistolCanvas").transform.Find("BulletamountText");
    }



    // Update is called once per frame
    void Update()
    {
        TouchSlideControl();
        KeyboardSlideControl();
    }
    private void TouchSlideControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FirstTouchToScreen();
        }
        else if (Input.GetMouseButton(0))
        {
            TouchingToScreen();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            LastTouchToScreen();
        }
    }

    private void LastTouchToScreen()
    {
        pistolXAxisMovePosition = 0;
    }

    private void TouchingToScreen()
    {
        pistolXAxisMovePosition = Input.mousePosition.x - mouseXPosition;
        mouseXPosition = Input.mousePosition.x;
        CalculatePistolNewXPosition(pistolXAxisMovePosition);
    }

    private void FirstTouchToScreen()
    {
        TouchedScreen(true);
        mouseXPosition = Input.mousePosition.x;
    }

    private void CalculatePistolNewXPosition(float newXPos)
    {
        pistolNewXPosition = Mathf.Clamp(transform.position.x + newXPos * pistolXAxisMovementAmount,
        pistolXAxisLimits.x,
        pistolXAxisLimits.y);
    }
    private void KeyboardSlideControl()
    {
        if (Input.GetButton("Horizontal"))
        {
            TouchedScreen(true);
            CalculatePistolNewXPosition(Input.GetAxisRaw("Horizontal"));
        }
    }



    private void FixedUpdate()
    {
        if (isPlayerAlive && isPlayerTouchedScreen)
        {
            MoveForward();
            WriteScreenBottom(pistolShootScript.bulletAmountInSecond.ToString() + "/sec");
        }
    }
    private void MoveForward()
    {
        pistolRigidbody.MovePosition(
            new Vector3(Mathf.Lerp(transform.position.x, pistolNewXPosition, pistolHorizontalSpeed * Time.fixedDeltaTime),
            transform.position.y,
            transform.position.z + pistolForwardSpeed * Time.fixedDeltaTime)
            );
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.CompareTag("Barrels"))
        {
            if (isLevelCompleted)
            {
                IncreaseLevelNumberByOne();
                SaveLevelNumberPlayerPrefs();
            }

            GameEnds();
        }
    }
    private void IncreaseLevelNumberByOne()
    {
        levelNumber++;
    }
    private void SaveLevelNumberPlayerPrefs()
    {
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
    }
    private void GameEnds()
    {
        PlayerAlive(false);
        SetFinishRigidbodyConstraints();
        canvasMenuScript.LevelCompleted(isLevelCompleted);
        cameraFollowScript.isFollowingTarget = false;
    }
    private void SetFinishRigidbodyConstraints()
    {
        pistolRigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        pistolRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (other.transform.position.z <= transform.position.z) 
                // rigidbody also detects children collisions instead of collision I compared position of Finish and pistol
            {
                IncreaseLevelNumberByOne();
                SaveLevelNumberPlayerPrefs();
                GameEnds();
            }
            
        }
    }
}