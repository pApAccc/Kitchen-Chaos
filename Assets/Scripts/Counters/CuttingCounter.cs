using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler OnAnyCut;
        public event EventHandler OnCut;
        public event EventHandler<IHasProgress.OnProgressBarChangedEventArgs> OnProgressBarChanged;

        [SerializeField] private KitchenObjectRecipeSO[] kitchenObjectRecipeSOArray;

        private int cuttingProgress;

        public static new void ResetStaticData()
        {
            OnAnyCut = null;
        }

        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                //都有物体
                //查看player拿的是不是Plate
                if (player.HasKitchenObject())
                {
                    if (player.GetKitchenObject().TryGetPlate(out PlateObject plateObject))
                    {
                        if (plateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        }
                        else
                        {
                            //plate上已经拥有此物体
                        }
                    }

                }
                else
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    KitchenObjectRecipeSO kitchenObjectOutput = GetKitchenObjectFormInput(player.GetKitchenObject().GetKitchenObjectSO());
                    if (kitchenObjectOutput != null)
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        InteractPutKitchenObjectOnCounterServerRpc();
                    }
                }
                else
                {

                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractPutKitchenObjectOnCounterServerRpc()
        {
            InteractPutKitchenObjectOnCounterClientRpc();
        }

        [ClientRpc]
        private void InteractPutKitchenObjectOnCounterClientRpc()
        {
            cuttingProgress = 0;

            OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
            {
                progressNormalized = 0
            });
        }


        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject())
            {
                CutObjectServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutObjectServerRpc()
        {
            CutObjectClientRpc();
            DestroyCutObjectServerRpc();
        }

        [ClientRpc]
        private void CutObjectClientRpc()
        {
            KitchenObjectRecipeSO kitchenObjectInput = GetKitchenObjectFormInput(GetKitchenObject().GetKitchenObjectSO());

            if (kitchenObjectInput != null)
            {
                cuttingProgress++;
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / kitchenObjectInput.cuttingProgressMax
                });
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyCutObjectServerRpc()
        {
            KitchenObjectRecipeSO kitchenObjectInput = GetKitchenObjectFormInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress >= kitchenObjectInput.cuttingProgressMax)
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpwanKitchenObject(kitchenObjectInput.output, this);
            }
        }

        private KitchenObjectRecipeSO GetKitchenObjectFormInput(KitchenObjectSO kitchenObjectSO)
        {
            foreach (KitchenObjectRecipeSO kitchenObjectRecipe in kitchenObjectRecipeSOArray)
            {
                if (kitchenObjectRecipe.input == kitchenObjectSO)
                {
                    return kitchenObjectRecipe;
                }
            }
            return null;
        }

    }


}
