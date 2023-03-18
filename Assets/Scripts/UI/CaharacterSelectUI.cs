using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CaharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuBtn;
        [SerializeField] private Button readyBtn;
        [SerializeField] TextMeshProUGUI lobbyNameText;
        [SerializeField] TextMeshProUGUI lobbyCodeText;

        private void Awake()
        {
            mainMenuBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.LeaveLobby();
                NetworkManager.Singleton.Shutdown();
                Loader.LoadScene(Loader.SceneName.GameMenuScene);
            });

            readyBtn.onClick.AddListener(() =>
            {
                CharacterReady.Instance.SetPlayerReady();
            });
        }

        private void Start()
        {
            Lobby joinedLobby = KitchenGameLobby.Instance.GetLobby();
            lobbyNameText.text = "房间名称: " + joinedLobby.Name;
            lobbyCodeText.text = "邀请码: " + joinedLobby.LobbyCode;
        }
    }
}
