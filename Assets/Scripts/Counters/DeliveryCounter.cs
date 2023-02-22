using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance;
        private void Awake()
        {
            Instance = this;
        }
        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateObject plateObject))
                {
                    DeliveryManager.Instance.DeliverRecipe(plateObject);
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}
