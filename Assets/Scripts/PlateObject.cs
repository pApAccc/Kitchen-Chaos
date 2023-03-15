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
    public class PlateObject : KitchenObject
    {

        public event EventHandler<OnIngridientAddedEventArgs> OnIngredientAdded;
        public class OnIngridientAddedEventArgs : EventArgs
        {
            public KitchenObjectSO kitchenObjectSO;
        }


        [SerializeField] private List<KitchenObjectSO> vaildKitchObjectSOList;

        private List<KitchenObjectSO> kitchenObjectSOList;
        protected override void Awake()
        {
            base.Awake();
            kitchenObjectSOList = new List<KitchenObjectSO>();
        }
        public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
        {
            if (!vaildKitchObjectSOList.Contains(kitchenObjectSO)) return false;

            if (kitchenObjectSOList.Contains(kitchenObjectSO))
            {
                return false;
            }
            else
            {
                AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));

                return true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddIngredientServerRpc(int kitchenObjectSOIndex)
        {
            AddIngredientClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        private void AddIngredientClientRpc(int kitchenObjectSOIndex)
        {
            KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectFromIndex(kitchenObjectSOIndex);
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngridientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
        }

        public List<KitchenObjectSO> GetKitchenObjectSOList()
        {
            return kitchenObjectSOList;
        }
    }
}
