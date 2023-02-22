using System.Collections;
using System.Collections.Generic;
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
        }

        public static SceneName targetScene;

        public static void LoadScene(SceneName targetScene)
        {
            if (SavingWrapper.Instance != null)
                SavingWrapper.Instance.Save();
            Loader.targetScene = targetScene;
            SceneManager.LoadSceneAsync(SceneName.LoadingScene.ToString());
        }

        public static IEnumerator LoadSceneCallback()
        {
            yield return SceneManager.LoadSceneAsync(targetScene.ToString());
        }

    }
}
