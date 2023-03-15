using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class WaittingOtherPlayersUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReady += Instance_OnLocalPlayerReady;
            GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
        }

        private void Instance_OnLocalPlayerReady(object sender, System.EventArgs e)
        {
            Show();
        }
        private void Instance_OnStateChanged(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
