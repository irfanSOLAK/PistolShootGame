using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasMenuScript : MonoBehaviour
{
    [Header("Canvas Menu")]
    public GameObject playerIsNotAliveMenu;
    public GameObject nextLevelMenu;

    private int levelNumberForCanvas;

    // Start is called before the first frame update
    void Start()
    {
        WriteLevelNumber();
        PlayerIsNotAliveMenuVisible(false);
        NextLevelMenuVisible(false);
    }
    private void WriteLevelNumber()
    {
        LoadLevelNumberForCanvas();
        transform.Find("LevelText").GetComponent<Text>().text = "Level " + levelNumberForCanvas.ToString();
    }

    private void LoadLevelNumberForCanvas()
    {
        levelNumberForCanvas = PlayerPrefs.GetInt("LevelNumber");

        if (levelNumberForCanvas < 1)
            levelNumberForCanvas = 1;
    }

    public void PlayerIsNotAliveMenuVisible(bool menuVisible)
    {
        playerIsNotAliveMenu.SetActive(menuVisible);
    }
    public void NextLevelMenuVisible(bool menuVisible)
    {
        nextLevelMenu.SetActive(menuVisible);
    }


    public void LevelCompleted(bool isCompleted)
    {
        NextLevelMenuVisible(isCompleted);
        PlayerIsNotAliveMenuVisible(!isCompleted);
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
