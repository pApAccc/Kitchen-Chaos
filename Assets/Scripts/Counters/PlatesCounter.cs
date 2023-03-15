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
    public class PlatesCounter : BaseCounter
    {
        public event EventHandler OnPlateSpawn;
        public event EventHandler OnPlateRemoved;
        private float spawnPlateTimer;
        private const float spawnPlateTimeMax = 4f;
        private int spawnPlateAmount;
        private const int spawnPlateAmountMax = 4;

        [SerializeField] private KitchenObjectSO plateSO;
        private void Update()
        {
            if (!IsServer) return;
            if (GameManager.Instance.GetIsWaittingPlayer()) return;

            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateAmount < spawnPlateAmountMax)
            {
                if (spawnPlateTimer > spawnPlateTimeMax)
                {
                    SpawnPlateClientRpc();
                }
            }
        }

        [ClientRpc]
        private void SpawnPlateClientRpc()
        {
            spawnPlateTimer = 0;
            spawnPlateAmount++;
            OnPlateSpawn?.Invoke(this, EventArgs.Empty);
        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                if (spawnPlateAmount > 0)
                {
                    KitchenObject.SpwanKitchenObject(plateSO, player);
                    InteractServerRpc();
                }
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
            spawnPlateAmount--;
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
