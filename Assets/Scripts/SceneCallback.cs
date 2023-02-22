using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class SceneCallback : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return Loader.LoadSceneCallback();
        }
    }
}
