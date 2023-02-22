using System;
using System.Collections;
using System.Collections.Generic;
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
        private void Awake()
        {
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
                kitchenObjectSOList.Add(kitchenObjectSO);
                OnIngredientAdded?.Invoke(this, new OnIngridientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
                return true;
            }
        }

        public List<KitchenObjectSO> GetKitchenObjectSOList()
        {
            return kitchenObjectSOList;
        }
    }
}
