using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuBtn;
        [SerializeField] private Button createLobbyBtn;
        [SerializeField] private Button quickJoinBtn;
        [SerializeField] private Button joinCodeBtn;
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private TMP_InputField playerNameInpuField;
        [SerializeField] private LobbyCreateUI lobbyCreateUI;
        [SerializeField] private Transform lobbycontainer;
        [SerializeField] private Transform lobbyTemplate;


        private void Awake()
        {
            mainMenuBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.LeaveLobby();
                Loader.LoadScene(Loader.SceneName.GameMenuScene);
            });

            createLobbyBtn.onClick.AddListener(() =>
            {
                lobbyCreateUI.Show();
            });

            quickJoinBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.QuickJoinGame();
            });

            joinCodeBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.JoinGameByCode(joinCodeInputField.text);
            });

            lobbyTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            playerNameInpuField.text = KitchenGameMultiplayer.Instance.GetPlayerName();
            playerNameInpuField.onValueChanged.AddListener((string newPlayerName) =>
            {
                KitchenGameMultiplayer.Instance.SetPLayerName(newPlayerName);
            });

            KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());
        }

        private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
        {
            UpdateLobbyList(e.lobbyList);
        }

        private void UpdateLobbyList(List<Lobby> lobbyList)
        {
            foreach (Transform child in lobbycontainer)
            {
                if (child == lobbyTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbyList)
            {
                Transform lobbyTransform = Instantiate(lobbyTemplate, lobbycontainer);
                lobbyTransform.gameObject.SetActive(true);
                lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
            }
        }

        private void OnDestroy()
        {
            KitchenGameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChanged;
        }
    }
}
