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
            Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
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
