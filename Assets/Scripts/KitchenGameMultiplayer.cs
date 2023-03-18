using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class KitchenGameMultiplayer : NetworkBehaviour
    {
        public const int MAX_PLAYER_AMOUNT = 4;
        private const string PLAYER_MULTIPLAYER_NAME = "PlayerNameMultiplayer";

        public event EventHandler OnTrytoJoinGame;
        public event EventHandler OnFailedtoJoinGame;
        public event EventHandler OnPlayerNetworkListChanged;

        public static KitchenGameMultiplayer Instance { get; private set; }
        private NetworkList<PlayerData> networkPlayerDataList;
        private string playerName;

        [SerializeField] private List<Color> playerColorList;
        [SerializeField] private KitchenObjectListSO kitchenObjectListSO;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerName = PlayerPrefs.GetString(PLAYER_MULTIPLAYER_NAME, "playerName" + Random.Range(10, 10000));
            networkPlayerDataList = new NetworkList<PlayerData>();
            networkPlayerDataList.OnListChanged += NetworkPlayerDataList_OnListChanged;
        }

        public string GetPlayerName() => playerName;

        public void SetPLayerName(string playerName)
        {
            this.playerName = playerName;

            PlayerPrefs.SetString(PLAYER_MULTIPLAYER_NAME, playerName);
        }

        private void NetworkPlayerDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
        {
            OnPlayerNetworkListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void StartHost()
        {
            //开启主机前添加连接检测
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworManager_ConnectionApprovalCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += NetworManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
            NetworkManager.Singleton.StartHost();
        }

        private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientID)
        {
            for (int i = 0; i < networkPlayerDataList.Count; i++)
            {
                if (clientID == networkPlayerDataList[i].clientID)
                {
                    //断连的client
                    networkPlayerDataList.RemoveAt(i);
                }
            }
        }

        private void NetworManager_OnClientConnectedCallback(ulong clientID)
        {
            networkPlayerDataList.Add(new PlayerData
            {
                clientID = clientID,
                colorID = GetFirstUnuseColorID()
            });
            SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);
            SetPlayerNameServerRpc(GetPlayerName());
        }

        private void NetworManager_ConnectionApprovalCallback(
            NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
            NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            if (SceneManager.GetActiveScene().name != Loader.SceneName.CharacterSelectScene.ToString())
            {
                connectionApprovalResponse.Approved = false;
                connectionApprovalResponse.Reason = "游戏已经开始了";
                return;
            }

            if (NetworkManager.Singleton.ConnectedClientsIds.Count > MAX_PLAYER_AMOUNT)
            {
                connectionApprovalResponse.Reason = "满员了";
                return;
            }
            connectionApprovalResponse.Approved = true;
        }

        public void StartClient()
        {
            OnTrytoJoinGame?.Invoke(this, EventArgs.Empty);

            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;

            NetworkManager.Singleton.StartClient();
        }
        public void SpwanKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
        {

            SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
        }
        private void NetworkManager_Client_OnClientConnectedCallback(ulong clientID)
        {
            SetPlayerNameServerRpc(GetPlayerName());
            SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);

        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientID(serverRpcParams.Receive.SenderClientId);
            PlayerData playerData = networkPlayerDataList[playerDataIndex];
            playerData.playerName = playerName;
            networkPlayerDataList[playerDataIndex] = playerData;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIDServerRpc(string playerID, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientID(serverRpcParams.Receive.SenderClientId);
            PlayerData playerData = networkPlayerDataList[playerDataIndex];
            playerData.playerID = playerID;
            networkPlayerDataList[playerDataIndex] = playerData;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkReference)
        {
            Transform kitchenObjectInstance = Instantiate(GetKitchenObjectFromIndex(kitchenObjectSOIndex).prefab);
            NetworkObject kitchenObjectNetworkObject = kitchenObjectInstance.GetComponent<NetworkObject>();
            kitchenObjectNetworkObject.Spawn(true);

            KitchenObject kitchenObject = kitchenObjectInstance.GetComponent<KitchenObject>();

            kitchenObjectParentNetworkReference.TryGet(out NetworkObject networkObject);
            IKitchenObjectParent kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        }

        public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO)
        {
            return kitchenObjectListSO.kitchenObjectSOList.IndexOf(kitchenObjectSO);
        }

        public KitchenObjectSO GetKitchenObjectFromIndex(int index)
        {
            return kitchenObjectListSO.kitchenObjectSOList[index];
        }

        public void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
        {
            kitchenObjectNetworkObjectReference.TryGet(out NetworkObject networkObject);
            KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();

            ClearKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);
            kitchenObject.DestroySelf();
        }

        [ClientRpc]
        private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
        {
            kitchenObjectNetworkObjectReference.TryGet(out NetworkObject networkObject);
            KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();

            kitchenObject.CleraKitchenObjectOnParent();
        }

        public bool IsPlayerConnected(int index)
        {
            return index < networkPlayerDataList.Count;
        }

        public PlayerData GetLocalPlayerData()
        {
            return GetPlayerDataFromClientID(NetworkManager.LocalClientId);
        }

        public PlayerData GetPlayerDataFromClientID(ulong clientID)
        {
            foreach (PlayerData playerData in networkPlayerDataList)
            {
                if (playerData.clientID == clientID)
                {
                    return playerData;
                }
            }
            return default;
        }
        public int GetPlayerDataIndexFromClientID(ulong clientID)
        {
            for (int i = 0; i < networkPlayerDataList.Count; i++)
            {
                if (networkPlayerDataList[i].clientID == clientID)
                {
                    return i;
                }
            }
            return -1;
        }

        public PlayerData GetPlayerData(int index)
        {
            return networkPlayerDataList[index];
        }

        public Color GetPlayerColor(int colorID)
        {
            return playerColorList[colorID];
        }

        public void ChangerPlayerColor(int colorIndex)
        {
            ChangerPlayerColorServerRpc(colorIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangerPlayerColorServerRpc(int colorIndex, ServerRpcParams serverRpcParams = default)
        {
            if (!IsColorAvaliable(colorIndex))
            {
                return;
            }

            int playerDataIndex = GetPlayerDataIndexFromClientID(serverRpcParams.Receive.SenderClientId);
            PlayerData playerData = networkPlayerDataList[playerDataIndex];
            playerData.colorID = colorIndex;
            networkPlayerDataList[playerDataIndex] = playerData;
        }

        private bool IsColorAvaliable(int colorIndex)
        {
            foreach (PlayerData playerData in networkPlayerDataList)
            {
                if (colorIndex == playerData.colorID)
                {
                    //颜色已经被别人使用了
                    return false;
                }
            }
            return true;
        }

        private int GetFirstUnuseColorID()
        {
            for (int i = 0; i < playerColorList.Count; i++)
            {
                if (IsColorAvaliable(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public void KickPlayer(ulong clientID)
        {
            NetworkManager.Singleton.DisconnectClient(clientID);
            NetworkManager_Server_OnClientDisconnectCallback(clientID);
        }
    }
}
