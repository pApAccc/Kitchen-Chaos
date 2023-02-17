using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        [SerializeField] private KitchenObjectRecipeSO[] kitchenObjectRecipeSOArray;
        private int cuttingProgress;

        public event EventHandler OnCut;
        public event EventHandler<IHasProgress.OnProgressBarChangedEventArgs> OnProgressBarChanged;
        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {

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
                        cuttingProgress = 0;

                        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                        {
                            progressNormalized = (float)cuttingProgress / kitchenObjectOutput.cuttingProgressMax
                        });
                    }
                }
                else
                {

                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject())
            {
                KitchenObjectRecipeSO kitchenObjectInput = GetKitchenObjectFormInput(GetKitchenObject().GetKitchenObjectSO());

                if (kitchenObjectInput != null)
                {
                    cuttingProgress++;
                    OnCut?.Invoke(this, EventArgs.Empty);
                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / kitchenObjectInput.cuttingProgressMax
                    });

                    if (cuttingProgress >= kitchenObjectInput.cuttingProgressMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpwanKitchenObject(kitchenObjectInput.output, this);
                    }
                }
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
