using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        private KitchenObject kitchenObject;

        [SerializeField] private Transform counterTopPoint;

        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter Interact");
        }
        public virtual void InteractAlternate(Player player)
        {
            //Debug.LogError("BaseCounter InteractAlternate");
        }

        public Transform GetHoldPointTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void ClearKitchenObject() => kitchenObject = null;

        public bool HasKitchenObject() => kitchenObject != null;
    }
}
