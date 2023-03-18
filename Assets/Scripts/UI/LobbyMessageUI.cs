using System;
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
    public class LobbyMessageUI : MonoBehaviour
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
            KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
            KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
            KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
            KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
            KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

            Hide();
        }

        private void KitchenGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
        {
            ShowMessage("快速加入游戏失败");
        }

        private void KitchenGameLobby_OnJoinFailed(object sender, EventArgs e)
        {
            ShowMessage("加入游戏失败");
        }

        private void KitchenGameLobby_OnJoinStarted(object sender, EventArgs e)
        {
            ShowMessage("加入游戏中....");
        }

        private void KitchenGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
        {
            ShowMessage("创建大厅....");
        }
        private void KitchenGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
        {
            ShowMessage("创建失败....");
        }
        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnFailedtoJoinGame -= KitchenGameMultiplayer_OnFailedtoJoinGame;
            KitchenGameLobby.Instance.OnCreateLobbyStarted -= KitchenGameLobby_OnCreateLobbyStarted;
            KitchenGameLobby.Instance.OnCreateLobbyFailed -= KitchenGameLobby_OnCreateLobbyFailed;
            KitchenGameLobby.Instance.OnJoinStarted -= KitchenGameLobby_OnJoinStarted;
            KitchenGameLobby.Instance.OnJoinFailed -= KitchenGameLobby_OnJoinFailed;
            KitchenGameLobby.Instance.OnQuickJoinFailed -= KitchenGameLobby_OnQuickJoinFailed;
        }

        private void KitchenGameMultiplayer_OnFailedtoJoinGame(object sender, System.EventArgs e)
        {
            if (NetworkManager.Singleton.DisconnectReason == "")
            {
                ShowMessage("连接失败");
            }
            else
            {
                ShowMessage(NetworkManager.Singleton.DisconnectReason);
            }
        }

        private void ShowMessage(string meassage)
        {
            Show();
            message.text = meassage;
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
