using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///当玩家死亡后，会加载当前关卡
///</summary>
namespace Common
{
    public class ReStartCurrentLevel
    {
        public static void ReLoadLevel()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex);
            Time.timeScale = 1.0f;
        }
    }
}
