using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private KitchenObjectSO plateSO;
        private float spawnPlateTimer;
        private const float spawnPlateTimeMax = 4f;
        private int spawnPlateAmount;
        private const int spawnPlateAmountMax = 4;
        private void Update()
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateAmount < spawnPlateAmountMax)
            {
                if (spawnPlateTimer > spawnPlateTimeMax)
                {
                    spawnPlateTimer = 0;

                    spawnPlateAmount++;
                    OnPlateSpawn?.Invoke(this, EventArgs.Empty);
                }
            }

        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                if (spawnPlateAmount > 0)
                {
                    KitchenObject.SpwanKitchenObject(plateSO, player);
                    spawnPlateAmount--;
                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
