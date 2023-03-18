using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class KitchenGameLobby : MonoBehaviour
    {
        private const string KET_RELAY_JOIN_CODE = "RelayJoinCode";

        public event EventHandler OnCreateLobbyStarted;
        public event EventHandler OnCreateLobbyFailed;
        public event EventHandler OnJoinStarted;
        public event EventHandler OnJoinFailed;
        public event EventHandler OnQuickJoinFailed;
        public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
        public class OnLobbyListChangedEventArgs : EventArgs
        {
            public List<Lobby> lobbyList;
        }

        public static KitchenGameLobby Instance { get; private set; }

        private Lobby joinedLobby;
        private float heartBeatTimer = 0;
        private float heartBeatTimerMax = 15;
        private float listLobbiesTimer;



        private void Awake()
        {
            Instance = this;
            heartBeatTimer = heartBeatTimerMax;
            DontDestroyOnLoad(gameObject);

            InitializeUnity();
        }

        private void Update()
        {
            HandleHeartBeat();
            HandlePeriodicListLobbies();
        }

        private void HandlePeriodicListLobbies()
        {
            if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn &&
                SceneManager.GetActiveScene().name == Loader.SceneName.LobbyScene.ToString())
            {
                listLobbiesTimer -= Time.deltaTime;
                if (listLobbiesTimer <= 0)
                {
                    float listLobbiesTimerMax = 3;
                    listLobbiesTimer = listLobbiesTimerMax;
                    ListLobbies();
                }
            }
        }

        private void HandleHeartBeat()
        {
            if (IsLobbyHost())
            {
                heartBeatTimer -= Time.deltaTime;
                if (heartBeatTimer <= 0)
                {
                    heartBeatTimer = heartBeatTimerMax;
                    LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        private bool IsLobbyHost()
        {
            return joinedLobby != null && AuthenticationService.Instance.PlayerId == joinedLobby.HostId;
        }

        private async void InitializeUnity()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                //保证在一台PC上测试正确，每次登入的玩家不一样
                InitializationOptions options = new InitializationOptions();
                options.SetProfile(Random.Range(1, 1000).ToString());
                await UnityServices.InitializeAsync(options);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        public async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                }
                };
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

                OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
                {
                    lobbyList = queryResponse.Results
                });
            }
            catch (LobbyServiceException e)
            {
                print(e);
            }

        }

        private async Task<Allocation> AllocateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(KitchenGameMultiplayer.MAX_PLAYER_AMOUNT - 1);
                return allocation;
            }
            catch (RelayServiceException e)
            {
                print(e);
                return default;
            }
        }

        private async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                string relayIoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return relayIoinCode;
            }
            catch (RelayServiceException e)
            {
                print(e);
                return default;
            }

        }
        private async Task<JoinAllocation> JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                return joinAllocation;
            }
            catch (RelayServiceException e)
            {
                print(e);
                return default;
            }
        }

        public async void CreateLobby(string lobbyName, bool isPrivate)
        {
            OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                //创建Lobby
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KitchenGameMultiplayer.MAX_PLAYER_AMOUNT,
                new CreateLobbyOptions
                {
                    IsPrivate = isPrivate
                });

                Allocation allocation = await AllocateRelay();
                string relayJoinCode = await GetRelayJoinCode(allocation);

                await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {KET_RELAY_JOIN_CODE,new DataObject(DataObject.VisibilityOptions.Member,relayJoinCode) }
                    }
                });

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

                //开启host
                KitchenGameMultiplayer.Instance.StartHost();
                Loader.LoadSceneNetwork(Loader.SceneName.CharacterSelectScene);
            }
            catch (LobbyServiceException e)
            {
                OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
                print(e);
            }
        }

        public async void QuickJoinGame()
        {
            OnJoinStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

                string relayJoinCode = joinedLobby.Data[KET_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                KitchenGameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                print(e);
                OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
            }

        }

        /// <summary>
        ///通过邀请码加入游戏 
        /// </summary>
        /// <param name="code"></param>
        public async void JoinGameByCode(string code)
        {
            OnJoinStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);

                string relayJoinCode = joinedLobby.Data[KET_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
                KitchenGameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                OnJoinFailed?.Invoke(this, EventArgs.Empty);
                print(e);
            }
        }

        public async void JoinGameByID(string id)
        {
            OnJoinStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(id);

                string relayJoinCode = joinedLobby.Data[KET_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                KitchenGameMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                OnJoinFailed?.Invoke(this, EventArgs.Empty);
                print(e);
            }
        }

        public async void DeleteLobby()
        {
            if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    print(e);
                }
            }
        }

        public async void LeaveLobby()
        {
            if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    print(e);
                }
            }
        }

        public async void KickPlayer(string playerID)
        {
            if (IsLobbyHost())
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerID);
                }
                catch (LobbyServiceException e)
                {
                    print(e);
                }
            }
        }

        public Lobby GetLobby() => joinedLobby;

    }
}
