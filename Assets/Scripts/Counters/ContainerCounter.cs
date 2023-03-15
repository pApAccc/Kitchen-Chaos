using System;
using Unity.Netcode;
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
                InteractServerRpc();

            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            InteractClientRpc();
        }

        [ClientRpc]
        private void InteractClientRpc()
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }

}
