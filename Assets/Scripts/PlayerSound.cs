using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlayerSound : MonoBehaviour
    {
        public static event EventHandler OnPlayerMoving;

        private Player player;
        private float timer;
        private float timerMax = .1f;

        private void Awake()
        {
            player = GetComponent<Player>();
            timer = timerMax;
        }
        private void Update()
        {
            if (!player.IsWalking()) return;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timerMax;
                OnPlayerMoving?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
