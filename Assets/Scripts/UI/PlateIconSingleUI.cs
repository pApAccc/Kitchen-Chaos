using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlateIconSingleUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void SetIconImage(KitchenObjectSO kitchenObjectSO)
        {
            image.sprite = kitchenObjectSO.sprite;
        }
    }
}
