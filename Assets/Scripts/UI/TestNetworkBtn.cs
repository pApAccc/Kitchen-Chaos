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
    public class TestNetworkBtn : MonoBehaviour
    {
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button clientBtn;

        private void Awake()
        {
            hostBtn.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                Hide();
            });

            clientBtn.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                Hide();
            });
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
