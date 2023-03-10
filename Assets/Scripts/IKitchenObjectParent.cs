using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public interface IKitchenObjectParent
    {
        void SetKitchenObject(KitchenObject kitchenObject);
        Transform GetHoldPointTransform();
        KitchenObject GetKitchenObject();
        void ClearKitchenObject();
        bool HasKitchenObject();
    }
}
