using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            BaseCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
            CuttingCounter.ResetStaticData();
            Player.ResetStaticData();
        }
    }
}
