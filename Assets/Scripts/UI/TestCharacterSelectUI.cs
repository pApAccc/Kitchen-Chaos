using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class TestCharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyBtn;

        private void Start()
        {
            readyBtn.onClick.AddListener(() =>
            {
                CharacterReady.Instance.SetPlayerReady();
            });
        }
    }
}
