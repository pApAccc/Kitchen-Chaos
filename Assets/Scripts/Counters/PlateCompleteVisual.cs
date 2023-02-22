using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [SerializeField] private PlateObject plateObject;
        [SerializeField] List<KitchObjectSO_GameObject> KitchObjectSO_GameObjectList;

        [Serializable]
        struct KitchObjectSO_GameObject
        {
            public KitchenObjectSO kitchenObjectSO;
            public GameObject gameObject;
        }
        private void Start()
        {
            plateObject.OnIngredientAdded += PlateObject_OnIngredientAdded;

            foreach (KitchObjectSO_GameObject kitchObjectSO_GameObject in KitchObjectSO_GameObjectList)
            {
                kitchObjectSO_GameObject.gameObject.SetActive(false);
            }
        }

        private void PlateObject_OnIngredientAdded(object sender, PlateObject.OnIngridientAddedEventArgs e)
        {
            foreach (KitchObjectSO_GameObject kitchObjectSO_GameObject in KitchObjectSO_GameObjectList)
            {
                if (e.kitchenObjectSO == kitchObjectSO_GameObject.kitchenObjectSO)
                {
                    kitchObjectSO_GameObject.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
