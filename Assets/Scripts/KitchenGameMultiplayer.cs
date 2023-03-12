using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
    }
}
