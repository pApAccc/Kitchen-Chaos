using System;

using UnityEngine;


/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ContainerCounter : BaseCounter
    {
        public event EventHandler OnInteract;

        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpwanKitchenObject(kitchenObjectSO, player);
                OnInteract?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
