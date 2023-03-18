using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CharacterSelectedColorSingleUI : MonoBehaviour
    {
        [SerializeField] private int colorIndex;
        [SerializeField] private Image image;
        [SerializeField] private GameObject selectedGameObject;


        private void Awake()
        {
            GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.ChangerPlayerColor(colorIndex);
            });
        }
        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnPlayerNetworkListChanged += KitchenGameMultiplayer_OnPlayerNetworkListChanged;
            image.color = KitchenGameMultiplayer.Instance.GetPlayerColor(colorIndex);
            UpdateSelectedVisual();
        }

        private void KitchenGameMultiplayer_OnPlayerNetworkListChanged(object sender, System.EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void UpdateSelectedVisual()
        {
            if (KitchenGameMultiplayer.Instance.GetLocalPlayerData().colorID == colorIndex)
            {
                selectedGameObject.SetActive(true);
            }
            else
            {
                selectedGameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnPlayerNetworkListChanged -= KitchenGameMultiplayer_OnPlayerNetworkListChanged;
        }
    }
}
