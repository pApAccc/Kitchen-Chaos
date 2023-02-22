using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlateIconsUI : MonoBehaviour
    {
        [SerializeField] private PlateObject plateObject;
        [SerializeField] private Transform plateIconUITemplate;
        private void Start()
        {
            plateObject.OnIngredientAdded += PlateObject_OnIngredientAdded;
        }

        private void PlateObject_OnIngredientAdded(object sender, PlateObject.OnIngridientAddedEventArgs e)
        {
            Transform plateIconUI = Instantiate(plateIconUITemplate, transform);
            plateIconUI.gameObject.SetActive(true);
            plateIconUI.GetComponent<PlateIconSingleUI>().SetIconImage(e.kitchenObjectSO);
        }
    }
}
