using System;
using System.Collections;
using System.Collections.Generic;
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
                player.GetKitchenObject().DestroySelf();
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
