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
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlacedHere;

        private KitchenObject kitchenObject;

        [SerializeField] private Transform counterTopPoint;

        public static void ResetStaticData()
        {
            OnAnyObjectPlacedHere = null;
        }

        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter Interact");
        }
        public virtual void InteractAlternate(Player player)
        {

        }

        public Transform GetHoldPointTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void ClearKitchenObject() => kitchenObject = null;

        public bool HasKitchenObject() => kitchenObject != null;

        public NetworkObject GetNetworkObject()
        {
            return null;
        }
    }
}
