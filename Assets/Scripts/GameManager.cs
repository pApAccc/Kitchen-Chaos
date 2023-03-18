using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameManager : NetworkBehaviour
    {
        public event EventHandler OnStateChanged;
        public event EventHandler OnLocalGamePaused;
        public event EventHandler OnLocalGameUnpaused;
        public event EventHandler OnLocalPlayerReady;
        public event EventHandler OnMultiplayerPause;
        public event EventHandler OnMultiplayerUnPause;

        public static GameManager Instance { get; private set; }
        private enum State
        {
            WaittingPlayer,
            WaittingToStart,
            CountDownToStart,
            GamePlaying,
            GameOver

        }

        private NetworkVariable<State> state = new NetworkVariable<State>(State.WaittingPlayer);
        private float waittingToStartTimer = 1;
        private NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3);
        private NetworkVariable<float> gamePlayTimer = new NetworkVariable<float>(0);
        private float gamePlayTimerMax = 180;
        private bool isLocalGamePause = false;
        private NetworkVariable<bool> isGamePause = new NetworkVariable<bool>(false);
        private bool isLocalPlayerReady;
        private bool isPlayerLeaveOnGameWaittingPlayer = false;

        private Dictionary<ulong, bool> playerReadyDictionary;
        private Dictionary<ulong, bool> playerPausedDictionary;

        [SerializeField] private Transform playerPrefab;
        private void Awake()
        {
            Instance = this;
            playerReadyDictionary = new Dictionary<ulong, bool>();
            playerPausedDictionary = new Dictionary<ulong, bool>();
        }
        private void Start()
        {
            GameInput.Instance.OnPause += GameInput_OnPause;
        }
        public override void OnNetworkSpawn()
        {
            //当游戏状态改变时
            state.OnValueChanged += (State previousState, State nowState) =>
            {
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            };
            gamePlayTimer.Value = gamePlayTimerMax;

            //当游戏暂停状态改变时
            isGamePause.OnValueChanged += (bool previousValue, bool newValue) =>
            {

                if (isGamePause.Value)
                {
                    Time.timeScale = 0;
                    OnMultiplayerPause?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Time.timeScale = 1;
                    OnMultiplayerUnPause?.Invoke(this, EventArgs.Empty);
                }
            };

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong disConnectedClientID)
        {
            //当有人断连时，检测游戏是否处于暂停状态
            if (state.Value == State.GamePlaying && playerPausedDictionary.ContainsKey(disConnectedClientID))
            {
                playerPausedDictionary.Remove(disConnectedClientID);
                CheckGameIsPause();
            }

            //进入游戏时断连,尝试开启游戏
            if (state.Value == State.WaittingPlayer)
            {
                isPlayerLeaveOnGameWaittingPlayer = true;
            }
        }

        private void SceneManager_OnLoadEventCompleted(
            string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode,
            List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform player = Instantiate(playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
            }
        }
        private void GameInput_OnPause(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerReadyDictionary.Add(serverRpcParams.Receive.SenderClientId, true);
            bool isAllReady = true;

            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientID) || playerReadyDictionary[clientID] == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            if (isAllReady)
            {
                state.Value = State.WaittingToStart;
            }
        }

        private void Update()
        {
            if (!isLocalPlayerReady)
            {
                isLocalPlayerReady = true;
                OnLocalPlayerReady?.Invoke(this, EventArgs.Empty);
                SetPlayerReadyServerRpc();
            }
            if (!IsServer) return;

            switch (state.Value)
            {
                case State.WaittingToStart:
                    waittingToStartTimer -= Time.deltaTime;
                    if (waittingToStartTimer < 0)
                    {
                        state.Value = State.CountDownToStart;
                    }
                    break;
                case State.CountDownToStart:
                    countDownToStartTimer.Value -= Time.deltaTime;
                    if (countDownToStartTimer.Value < 0)
                    {
                        state.Value = State.GamePlaying;
                    }
                    break;
                case State.GamePlaying:
                    gamePlayTimer.Value -= Time.deltaTime;
                    if (gamePlayTimer.Value < 0)
                    {
                        state.Value = State.GameOver;
                    }
                    break;
                case State.GameOver:
                    break;
            }
        }

        private void LateUpdate()
        {
            if (isPlayerLeaveOnGameWaittingPlayer)
            {
                TryStartGameOnPlayerLeave();
                isPlayerLeaveOnGameWaittingPlayer = false;
            }
        }

        private void TryStartGameOnPlayerLeave()
        {
            bool isAllReady = true;
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientID) || playerReadyDictionary[clientID] == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            if (isAllReady)
            {
                state.Value = State.WaittingToStart;
            }
        }

        public void TogglePauseGame()
        {
            isLocalGamePause = !isLocalGamePause;
            if (isLocalGamePause)
            {
                PauseGameServerRpc();
                OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
            }

            else
            {
                UnPauseGameServerRpc();
                OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

            CheckGameIsPause();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

            CheckGameIsPause();
        }

        private void CheckGameIsPause()
        {
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (playerPausedDictionary.ContainsKey(clientID) && playerPausedDictionary[clientID] == true)
                {
                    //有人暂停了游戏
                    isGamePause.Value = true;
                    return;
                }
            }
            //游戏正常运行
            isGamePause.Value = false;
        }

        public bool IsGamePlaying()
        {
            return state.Value == State.GamePlaying;
        }

        public bool IsCountDownToStart()
        {
            return state.Value == State.CountDownToStart;
        }

        public float GetCountDownToStartTimer()
        {
            return countDownToStartTimer.Value;
        }

        public bool IsGameOver()
        {
            return state.Value == State.GameOver;
        }

        public float GetGamePlayTimerNormailzed() => gamePlayTimer.Value / gamePlayTimerMax;

        public bool GetIsLocalPlayerReady() => isLocalPlayerReady;

        public bool GetIsWaittingPlayer() => state.Value == State.WaittingPlayer;
    }
}
