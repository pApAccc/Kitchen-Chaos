using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyObjectTrashed;
        public static new void ResetStaticData()
        {
            OnAnyObjectTrashed = null;
        }
        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
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
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
