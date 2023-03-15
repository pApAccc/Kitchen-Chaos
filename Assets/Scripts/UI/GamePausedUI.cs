using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GamePausedUI : MonoBehaviour
    {
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button optionsBtn;
        [SerializeField] private Button backMenuBtn;
        private void Start()
        {
            GameManager.Instance.OnLocalGamePaused += GameManager_OnGameLocalPaused;
            GameManager.Instance.OnLocalGameUnpaused += GameManager_OnGameLocalUnpaused;

            resumeBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.TogglePauseGame();
            });

            backMenuBtn.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.SceneName.GameMenuScene);
            });
            optionsBtn.onClick.AddListener(() =>
            {
                OptionsUI.Instance.Show();
            });

            Hide();
        }

        private void GameManager_OnGameLocalUnpaused(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void GameManager_OnGameLocalPaused(object sender, System.EventArgs e)
        {
            Show();
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
