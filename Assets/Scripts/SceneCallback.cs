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
        private void Start()
        {
            Loader.LoadSceneCallback();
        }
    }
}
