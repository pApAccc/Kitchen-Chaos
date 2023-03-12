using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameManager : MonoBehaviour
    {
        public event EventHandler OnStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameUnpaused;
        public static GameManager Instance { get; private set; }
        private enum State
        {
            WaittingToStart,
            CountDownToStart,
            GamePlaying,
            GameOver
        }
        private State state;
        private float waittingToStartTimer = 1;
        private float countDownToStartTimer = 3;
        private float gamePlayTimer;
        private float gamePlayTimerMax = 600;
        private bool isGamePause = false;
        private void Awake()
        {
            Instance = this;
            state = State.WaittingToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            gamePlayTimer = gamePlayTimerMax;
        }
        private void Start()
        {
            GameInput.Instance.OnPause += GameInput_OnPause;
        }

        private void GameInput_OnPause(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        public void TogglePauseGame()
        {
            isGamePause = !isGamePause;
            if (isGamePause)
            {
                Time.timeScale = 0;
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }

            else
            {
                Time.timeScale = 1;
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }

        }

        private void Update()
        {
            switch (state)
            {
                case State.WaittingToStart:
                    waittingToStartTimer -= Time.deltaTime;
                    if (waittingToStartTimer < 0)
                    {
                        state = State.CountDownToStart;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.CountDownToStart:
                    countDownToStartTimer -= Time.deltaTime;
                    if (countDownToStartTimer < 0)
                    {
                        state = State.GamePlaying;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.GamePlaying:
                    gamePlayTimer -= Time.deltaTime;
                    if (gamePlayTimer < 0)
                    {
                        state = State.GameOver;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.GameOver:
                    break;
            }

        }
        public bool IsGamePlaying()
        {
            return state == State.GamePlaying;
        }

        public bool IsCountDownToStart()
        {
            return state == State.CountDownToStart;
        }

        public float GetCountDownToStartTimer()
        {
            return countDownToStartTimer;
        }

        public bool IsGameOver()
        {
            return state == State.GameOver;
        }

        public float GetGamePlayTimerNormailzed() => gamePlayTimer / gamePlayTimerMax;


    }
}
