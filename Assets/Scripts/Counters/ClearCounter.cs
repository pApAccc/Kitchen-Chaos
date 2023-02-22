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
                    //查看player拿的是不是Plate
                    if (player.GetKitchenObject().TryGetPlate(out PlateObject playerPlateObject))
                    {
                        if (playerPlateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                        }
                        else
                        {
                            //plate上已经拥有此物体
                        }
                    }
                    //查看ClearCounter上的是不是Plate
                    else
                    {
                        if (GetKitchenObject().TryGetPlate(out playerPlateObject))
                        {
                            if (playerPlateObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            {
                                player.GetKitchenObject().DestroySelf();
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}

