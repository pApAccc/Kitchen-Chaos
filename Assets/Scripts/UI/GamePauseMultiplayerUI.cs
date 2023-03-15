using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GamePauseMultiplayerUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnMultiplayerPause += GameManager_OnMultiplayerPause;
            GameManager.Instance.OnMultiplayerUnPause += GameManager_OnMultiplayerUnPause;

            Hide();
        }


        private void GameManager_OnMultiplayerPause(object sender, System.EventArgs e)
        {
            Show();
        }

        private void GameManager_OnMultiplayerUnPause(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
