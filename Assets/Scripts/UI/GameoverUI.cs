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
    public class GameoverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deliveredRecipeNumText;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button quitBtn;
        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameMnagaer_OnStateChanged;

            restartBtn.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.LoadScene(Loader.SceneName.GameScene);
            });
            quitBtn.onClick.AddListener(() => { Application.Quit(); });

            Hide();
        }

        private void GameMnagaer_OnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsGameOver())
            {
                Show();
                deliveredRecipeNumText.text = DeliveryManager.Instance.GetRecipesHasDeliveredAmount().ToString();
            }
            else
            {
                Hide();
            }
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

    }
}
