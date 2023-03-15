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

            }


        }

        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
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
