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
    public class CharacterSelectPlayer : MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private GameObject readyGameObject;
        [SerializeField] private PlayerVisual playerVisual;
        [SerializeField] private Button kickBtn;
        [SerializeField] private TextMeshProUGUI playerNameText;

        private void Awake()
        {
            kickBtn.onClick.AddListener(() =>
            {
                PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerData(playerIndex);
                KitchenGameLobby.Instance.KickPlayer(playerData.playerID.ToString());
                KitchenGameMultiplayer.Instance.KickPlayer(playerData.clientID);
            });
        }

        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnPlayerNetworkListChanged += KitchenGameMultiplayer_OnPlayerNetworkListChanged;

            CharacterReady.Instance.OnReadyChanged += CharacterReady_OnReadyChanged;

            kickBtn.gameObject.SetActive(NetworkManager.Singleton.IsServer);

            UpdatePlayer();
        }

        private void KitchenGameMultiplayer_OnPlayerNetworkListChanged(object sender, System.EventArgs e)
        {
            UpdatePlayer();
        }

        private void CharacterReady_OnReadyChanged(object sender, System.EventArgs e)
        {
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            if (KitchenGameMultiplayer.Instance.IsPlayerConnected(playerIndex))
            {
                Show();

                PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerData(playerIndex);
                readyGameObject.SetActive(CharacterReady.Instance.GetPlayerIsReady(playerData.clientID));

                playerNameText.text = playerData.playerName.ToString();

                playerVisual.SetColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorID));
            }
            else
            {
                Hide();
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
            CharacterReady.Instance.OnReadyChanged -= CharacterReady_OnReadyChanged;
            KitchenGameMultiplayer.Instance.OnPlayerNetworkListChanged -= KitchenGameMultiplayer_OnPlayerNetworkListChanged;
        }
    }
}
