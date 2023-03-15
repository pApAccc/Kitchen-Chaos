using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public static class Loader
    {
        public enum SceneName
        {
            GameScene,
            GameMenuScene,
            LoadingScene,
            LobbyScene,
            CharacterSelectScene
        }

        public static SceneName targetScene;

        public static void LoadScene(SceneName targetScene)
        {
            if (SavingWrapper.Instance != null)
                SavingWrapper.Instance.Save();
            Loader.targetScene = targetScene;
            SceneManager.LoadSceneAsync(SceneName.LoadingScene.ToString());
        }

        //public static IEnumerator LoadSceneCallback()
        //{
        //    yield return SceneManager.LoadSceneAsync(targetScene.ToString());
        //}

        public static void LoadSceneNetwork(SceneName targetScene)
        {
            if (SavingWrapper.Instance != null)
                SavingWrapper.Instance.Save();
            Loader.targetScene = targetScene;
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
        }

        public static async void LoadSceneCallback()
        {
            await SceneManager.LoadSceneAsync(targetScene.ToString());
        }

        public static void LoadSceneNetworkWithLoadingScene(SceneName targetScene)
        {
            Loader.targetScene = targetScene;
            NetworkManager.Singleton.SceneManager.LoadScene(SceneName.LoadingScene.ToString(), LoadSceneMode.Single);
        }

    }
}
