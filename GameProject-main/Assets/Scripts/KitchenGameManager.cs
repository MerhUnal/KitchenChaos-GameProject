using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnGamePlayingTimerChanged;

   

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        LevelComplete,
        GameOver,
    }

    private State state;

    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 60f;
    private bool isGamePaused = false;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        int currentLevelIndex = LevelSystem.MainLevelSystem.GetCurrentLevelIndex();

        if (currentLevelIndex >= 0 && currentLevelIndex < LevelSystem.MainLevelSystem.levelDatas.levelDatas.Count)
        {
            gamePlayingTimerMax = LevelSystem.MainLevelSystem.GetLevelTime(currentLevelIndex);
        }
        else
        {
            Debug.LogError("Invalid level index: " + currentLevelIndex);
        }
    }

    public void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                
                break;
            case State.CountDownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                OnGamePlayingTimerChanged?.Invoke(this, EventArgs.Empty);


                if (gamePlayingTimer < 0f)
                {
                    if (DeliveryManager.Instance.GetsuccessfulRecipesAmount() >= 4)
                    {
                        state = State.LevelComplete;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        state = State.GameOver;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                break;
            case State.LevelComplete:
                break;
            case State.GameOver:
                break;
        }
    }

  


    public void HandleLevelComplete()
    {
        int currentLevelIndex = LevelSystem.MainLevelSystem.GetCurrentLevelIndex();
        LevelSystem.MainLevelSystem.SetNextLevel();
        int nextLevelIndex = LevelSystem.MainLevelSystem.GetCurrentLevelIndex();

        // Yeni seviyeyi ba?latmak için sahneyi güncelleyin
        string nextSceneName = LevelSystem.MainLevelSystem.GetCurrentLevelScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);

        // Yeni seviyeye ait süreyi al ve zamanlay?c?y? ayarla
        
        gamePlayingTimerMax = LevelSystem.MainLevelSystem.GetLevelTime(nextLevelIndex);
        countdownToStartTimer = 3f;
        //state = State.CountDownToStart;
        //OnStateChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsLevelComplete()
    {
        return state == State.LevelComplete;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetGamePlayingTimer()
    {
        return gamePlayingTimer;
    }

    public void AddTime(float timeToAdd)
    {
        gamePlayingTimer += timeToAdd;

        if (gamePlayingTimer > 60f)
        {
            gamePlayingTimerMax = gamePlayingTimer;
        }
    }


    public bool IsFireScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName == "Stage2" || sceneName == "Stage3";
    }
}
