using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class KitchenGameMultiplayer : NetworkBehaviour
    {
        private const int MAX_PLAYER_AMOUNT = 4;

        public event EventHandler OnTrytoJoinGame;
        public event EventHandler OnFailedtoJoinGame;
        public static KitchenGameMultiplayer Instance { get; private set; }
        [SerializeField] private KitchenObjectListSO kitchenObjectListSO;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartHost()
        {
            //开启主机前添加连接检测
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworManager_ConnectionApprovalCallback;
            NetworkManager.Singleton.StartHost();
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

            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.StartClient();
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong obj)
        {
            OnFailedtoJoinGame?.Invoke(this, EventArgs.Empty);
        }

        public void SpwanKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
        {

            SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
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

    }
}
