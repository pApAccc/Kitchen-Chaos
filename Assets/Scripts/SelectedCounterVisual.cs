using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter clearCounter;
        [SerializeField] private GameObject[] counterVisual;
        private void Start()
        {
            if (Player.LocalInstance != null)
            {
                Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
            else
            {
                Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
            }

        }

        private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
        {
            if (Player.LocalInstance != null)
            {
                Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
                Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
        }

        private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
        {
            if (clearCounter == e.selectedCounter)
            {
                foreach (var counterVisual in counterVisual)
                    counterVisual.SetActive(true);
            }
            else
            {
                foreach (var counterVisual in counterVisual)
                    counterVisual.SetActive(false);
            }
        }
    }
}
