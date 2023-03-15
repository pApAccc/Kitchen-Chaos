using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameMenuUI : MonoBehaviour
    {

        [SerializeField] Button gameStartBtn;
        [SerializeField] Button gameQuitBtn;
        private void Awake()
        {
            gameStartBtn.onClick.AddListener(() =>
            {
                Loader.LoadScene(Loader.SceneName.LobbyScene);
            });

            gameQuitBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });

            Time.timeScale = 1.0f;
        }
    }
}
