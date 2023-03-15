using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class KitchenObject : NetworkBehaviour
    {
        private IKitchenObjectParent kitchenObjectParent;
        private FollowTransform followTransform;

        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        protected virtual void Awake()
        {
            followTransform = GetComponent<FollowTransform>();
        }
        public KitchenObjectSO GetKitchenObjectSO()
        {
            return kitchenObjectSO;
        }

        public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObject)
        {
            SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObject);
        }

        [ClientRpc]
        private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObject)
        {
            kitchenObjectParentNetworkObject.TryGet(out NetworkObject networkObject);
            IKitchenObjectParent kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();

            //设置旧字段
            this.kitchenObjectParent?.ClearKitchenObject();
            this.kitchenObjectParent = kitchenObjectParent;

            //设置新字段
            kitchenObjectParent.SetKitchenObject(this);
            followTransform.SetTargetTransform(kitchenObjectParent.GetHoldPointTransform());
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void CleraKitchenObjectOnParent()
        {
            kitchenObjectParent.ClearKitchenObject();
        }

        /// <summary>
        /// 尝试将KitchenObject转化为PlateObject
        /// </summary>
        /// <param name="plateObject"></param>
        /// <returns></returns>
        public bool TryGetPlate(out PlateObject plateObject)
        {
            if (this is PlateObject)
            {
                plateObject = (PlateObject)this;
                return true;
            }
            else
            {
                plateObject = null;
                return false;
            }
        }

        public static void SpwanKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
        {
            KitchenGameMultiplayer.Instance.SpwanKitchenObject(kitchenObjectSO, kitchenObjectParent);
        }

        public static void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
        }



    }
}
