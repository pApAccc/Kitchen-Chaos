using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ConnectResponseMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button closeBtn;

        private void Awake()
        {
            closeBtn.onClick.AddListener(Hide);
        }

        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnFailedtoJoinGame += KitchenGameMultiplayer_OnFailedtoJoinGame;
            Hide();
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnFailedtoJoinGame -= KitchenGameMultiplayer_OnFailedtoJoinGame;
        }

        private void KitchenGameMultiplayer_OnFailedtoJoinGame(object sender, System.EventArgs e)
        {
            Show();
            message.text = NetworkManager.Singleton.DisconnectReason;

            if (message.text == "")
            {
                message.text = "连接失败";
            }
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
