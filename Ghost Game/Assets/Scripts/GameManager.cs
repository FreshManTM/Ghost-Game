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
    public CanvasGroup canvasWin;
    public CanvasGroup canvasLose;
    public bool isDialog;

    public bool gameIsOver;

    public event EventHandler BombExploded;
    public static GameManager gm;

    [SerializeField] AudioSource spotSound;
    [SerializeField] AudioSource winSound;

    bool isFadingWin;
    bool isFadingLose;
    void Start()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1;

        hasKey = false;
        inRangeOfDoor = false;
        EnemyAI.OnEnemyHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedEndOfLevel += ShowGameWinUI;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape) && !gameIsOver)
        {
            PauseButton();
            canvasPause.SetActive(true);
        }
        Fading();
    }

    private void Fading()
    {
        if (isFadingWin)
        {
            canvasWin.alpha += Time.deltaTime * .5f;
            if (canvasWin.alpha == 1)
                isFadingWin = false;
        }
        if (isFadingLose)
        {
            canvasLose.alpha += Time.deltaTime * .5f;
            if (canvasLose.alpha == 1)
                isFadingLose = false;
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
        PlayerPrefs.SetInt("levelAt", SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.None;

        winSound.Play();
        OnGameOver(canvasWin);
        isFadingWin = true;
    }
    void ShowGameLoseUI()
    {
        spotSound.Play();
        Cursor.lockState = CursorLockMode.None;

        OnGameOver(canvasLose);
        isFadingLose = true;
    }
    void OnGameOver(CanvasGroup gameOverUI)
    {
        gameOverUI.gameObject.SetActive(true);
        gameIsOver = true;
        EnemyAI.OnEnemyHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<PlayerController>().OnReachedEndOfLevel -= ShowGameWinUI;

    }
    public void PauseButton()
    {
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;
        paused = true;
    }
    public void ResumeButton()
    {
        Cursor.lockState = CursorLockMode.Locked;

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
