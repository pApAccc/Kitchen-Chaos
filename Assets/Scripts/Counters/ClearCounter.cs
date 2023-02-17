using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                //counter没有物体
                if (player.HasKitchenObject())
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                else
                {
                    //都没有物体
                    return;
                }
            }
            else
            {
                //counter有物体
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
                else
                {
                    //都有物体
                    return;
                }
            }
        }
    }
}

