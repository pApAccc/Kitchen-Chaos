using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ConnectingUI : MonoBehaviour
    {
        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnTrytoJoinGame += KitchenGameMultiplayer_OnTrytoJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedtoJoinGame += KitchenGameMultiplayer_OnFailedtoJoinGame;
            Hide();
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnTrytoJoinGame -= KitchenGameMultiplayer_OnTrytoJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedtoJoinGame -= KitchenGameMultiplayer_OnFailedtoJoinGame;
        }

        private void KitchenGameMultiplayer_OnFailedtoJoinGame(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void KitchenGameMultiplayer_OnTrytoJoinGame(object sender, System.EventArgs e)
        {
            Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
