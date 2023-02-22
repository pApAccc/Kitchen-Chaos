using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GamePlayTimer : MonoBehaviour
    {
        [SerializeField] private Image timerImage;

        private void Update()
        {
            timerImage.fillAmount = GameManager.Instance.GetGamePlayTimerNormailzed();
        }
    }
}
