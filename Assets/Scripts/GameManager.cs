using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject startScreen;
    public GameObject gameOverScreen;
    public GameObject inGameUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        inGameUI.SetActive(true);
        isGameActive = true;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        isGameActive=false;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
