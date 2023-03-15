
using System;
using Unity.Netcode;
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

        private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
        private KitchenFringRecipeSO kitchenFringRecipeSO;
        private KitchenBuringRecipeSO kitchenBuringRecipeSO;
        private NetworkVariable<float> fringTimer = new NetworkVariable<float>(0);
        private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0);

        public override void OnNetworkSpawn()
        {
            fringTimer.OnValueChanged += (float previousValue, float value) =>
            {
                float fryingTiemrMax = kitchenFringRecipeSO != null ? kitchenFringRecipeSO.cookingTimeMax : 1f;

                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressNormalized = fringTimer.Value / fryingTiemrMax
                });
            };

            burningTimer.OnValueChanged += (float previousValue, float value) =>
            {
                float burningTiemrMax = kitchenBuringRecipeSO != null ? kitchenBuringRecipeSO.buringTimeMax : 1f;

                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressNormalized = burningTimer.Value / burningTiemrMax
                });
            };

            state.OnValueChanged += (State previouseState, State newState) =>
            {
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state.Value });

                if (state.Value == State.Burned || state.Value == State.Idle)
                {
                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                }
            };
        }
        private void Update()
        {
            if (!IsServer) return;

            if (HasKitchenObject())
            {
                switch (state.Value)
                {
                    case State.Idle:

                        break;
                    case State.Fring:
                        fringTimer.Value += Time.deltaTime;

                        if (fringTimer.Value > kitchenFringRecipeSO.cookingTimeMax)
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            KitchenObject.SpwanKitchenObject(kitchenFringRecipeSO.output, this);

                            state.Value = State.Fired;

                            burningTimer.Value = 0;
                            SetBurningRecipeSOClientRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));
                        }
                        break;
                    case State.Fired:
                        burningTimer.Value += Time.deltaTime;

                        if (burningTimer.Value > kitchenBuringRecipeSO.buringTimeMax)
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            KitchenObject.SpwanKitchenObject(kitchenBuringRecipeSO.output, this);

                            state.Value = State.Burned;
                        }
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
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());

                            PickupObjectServerRpc();
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
                    PickupObjectServerRpc();
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    KitchenFringRecipeSO kitchenFringInput = GetKitchenFringFomInput(player.GetKitchenObject().GetKitchenObjectSO());
                    if (kitchenFringInput != null)
                    {
                        KitchenObject kitchenObject = player.GetKitchenObject();
                        kitchenObject.SetKitchenObjectParent(this);

                        InteractPlaceKitchenObjectServerRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO()));
                    }
                }
                else
                {

                }
            }

        }

        [ServerRpc(RequireOwnership = false)]
        private void PickupObjectServerRpc()
        {
            state.Value = State.Idle;
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractPlaceKitchenObjectServerRpc(int index)
        {
            fringTimer.Value = 0;
            state.Value = State.Fring;
            SetFringRecipeSOClientRpc(index);
        }

        [ClientRpc]
        private void SetFringRecipeSOClientRpc(int index)
        {
            kitchenFringRecipeSO = GetKitchenFringFomInput(KitchenGameMultiplayer.Instance.GetKitchenObjectFromIndex(index));
        }

        [ClientRpc]
        private void SetBurningRecipeSOClientRpc(int index)
        {
            kitchenBuringRecipeSO = GetKitchenBuringFormInput(KitchenGameMultiplayer.Instance.GetKitchenObjectFromIndex(index));
        }

        private KitchenFringRecipeSO GetKitchenFringFomInput(KitchenObjectSO kitchenObjectSO)
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

        public bool IsFired() => state.Value == State.Fired;

    }
}
