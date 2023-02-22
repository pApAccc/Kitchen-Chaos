
using System;
using UnityEngine;
using static UnityEngine.CullingGroup;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
        public event EventHandler<IHasProgress.OnProgressBarChangedEventArgs> OnProgressBarChanged;

        public class OnStateChangedEventArgs : EventArgs
        {
            public State state;
        }

        public enum State
        {
            Idle,
            Fring,
            Fired,
            Burned
        }

        [SerializeField] private KitchenFringRecipeSO[] kitchenFringRecipeSOArray;
        [SerializeField] private KitchenBuringRecipeSO[] kitchenBuringRecipeSOArray;

        private State state;
        private KitchenFringRecipeSO kitchenFringRecipeSO;
        private KitchenBuringRecipeSO kitchenBuringRecipeSO;
        private float fringTimer;
        private float buringTimer;

        private void Start()
        {
            state = State.Idle;

        }
        private void Update()
        {
            if (HasKitchenObject())
            {
                switch (state)
                {
                    case State.Idle:

                        break;
                    case State.Fring:
                        fringTimer += Time.deltaTime;
                        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                        {
                            progressNormalized = fringTimer / kitchenFringRecipeSO.cookingTimeMax
                        });

                        if (fringTimer > kitchenFringRecipeSO.cookingTimeMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpwanKitchenObject(kitchenFringRecipeSO.output, this);

                            state = State.Fired;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                            buringTimer = 0;
                            kitchenBuringRecipeSO = GetKitchenBuringFormInput(GetKitchenObject().GetKitchenObjectSO());
                        }
                        break;
                    case State.Fired:
                        buringTimer += Time.deltaTime;

                        if (buringTimer > kitchenBuringRecipeSO.buringTimeMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpwanKitchenObject(kitchenBuringRecipeSO.output, this);

                            state = State.Burned;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        }

                        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                        {
                            progressNormalized = buringTimer / kitchenBuringRecipeSO.buringTimeMax
                        });
                        break;
                    case State.Burned:
                        break;
                }

            }
        }
        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                //查看player拿的是不是Plate
                if (player.HasKitchenObject())
                {
                    if (player.GetKitchenObject().TryGetPlate(out PlateObject plateObject))
                    {
                        if (plateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();

                            state = State.Idle;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                            OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                            {
                                progressNormalized = 0
                            });
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
                    state = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    KitchenFringRecipeSO kitchenFringInput = GetKitchenFringFormInput(player.GetKitchenObject().GetKitchenObjectSO());
                    if (kitchenFringInput != null)
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        kitchenFringRecipeSO = GetKitchenFringFormInput(GetKitchenObject().GetKitchenObjectSO());

                        state = State.Fring;
                        fringTimer = 0;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                }
                else
                {

                }
            }

        }

        private KitchenFringRecipeSO GetKitchenFringFormInput(KitchenObjectSO kitchenObjectSO)
        {
            foreach (KitchenFringRecipeSO kitchenFringRecipe in kitchenFringRecipeSOArray)
            {
                if (kitchenFringRecipe.input == kitchenObjectSO)
                {
                    return kitchenFringRecipe;
                }
            }
            return null;
        }
        private KitchenBuringRecipeSO GetKitchenBuringFormInput(KitchenObjectSO kitchenObjectSO)
        {
            foreach (KitchenBuringRecipeSO kitchenBuringRecipe in kitchenBuringRecipeSOArray)
            {
                if (kitchenBuringRecipe.input == kitchenObjectSO)
                {
                    return kitchenBuringRecipe;
                }
            }
            return null;
        }

        public bool IsFired() => state == State.Fired;

    }
}
