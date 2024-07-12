using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    
    public static KitchenGameManager Instance {  get; private set; }

    public event EventHandler OnstateChanged;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 10f;

    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
          case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountDownToStart;
                    OnstateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
          case State.CountDownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
          case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
          case State.GameOver:
                break;
        }
        Debug.Log(state);
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
}
