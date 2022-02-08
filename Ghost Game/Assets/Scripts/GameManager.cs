using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public bool hasKey;
    public bool inRangeOfDoor;
    bool paused;
    [SerializeField] GameObject canvasPause;
    public GameObject canvasWin;
    public GameObject canvasLose;
    public bool isDialog;

    public bool gameIsOver;

    public event EventHandler BombExploded;
    public static GameManager gm;

    void Start()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1;

        hasKey = false;
        inRangeOfDoor = false;
        EnemyAI.OnEnemyHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedEndOfLevel += ShowGameWinUI;
    }


    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape) && !gameIsOver)
        {
            PauseButton();
            canvasPause.SetActive(true);
        }

    }
    private void Awake()
    {
        gm = this;
    }
    public virtual void Explode(Explosion ex)
    {
        BombExploded?.Invoke(ex, null);
    }
    void ShowGameWinUI()
    {
        OnGameOver(canvasWin);
    }
    void ShowGameLoseUI()
    {
        OnGameOver(canvasLose);
    }
    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        EnemyAI.OnEnemyHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedEndOfLevel -= ShowGameWinUI;

    }
    public void PauseButton()
    {
        Time.timeScale = 0;
        paused = true;
    }
    public void ResumeButton()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
