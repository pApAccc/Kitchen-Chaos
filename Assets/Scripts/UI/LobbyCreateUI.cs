using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class LobbyCreateUI : MonoBehaviour
    {
        [SerializeField] private Button privateBtn;
        [SerializeField] private Button publicBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private TMP_InputField lobbyNameInputField;

        private void Awake()
        {
            privateBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
            });

            publicBtn.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
            });

            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });

        }

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
