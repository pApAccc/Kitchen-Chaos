using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class HostQuitUI : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;

        private void Awake()
        {
            playAgainButton.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.SceneName.GameMenuScene);
            });
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

            Hide();
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
        {
            if (clientID == NetworkManager.ServerClientId)
            {
                Show();
            }
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }
}
