using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class TestLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button createGameBtn;
        [SerializeField] private Button joinGameBtn;

        private void Awake()
        {
            createGameBtn.onClick.AddListener(() =>
            {

                KitchenGameMultiplayer.Instance.StartHost();
                Loader.LoadSceneNetwork(Loader.SceneName.CharacterSelectScene);
            });

            joinGameBtn.onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.StartClient();
            });
        }
    }
}
