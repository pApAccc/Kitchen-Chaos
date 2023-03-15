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
                KitchenGameMultiplayer.Instance.StartHost();
                Hide();
            });

            clientBtn.onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.StartClient();
                Hide();
            });
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
