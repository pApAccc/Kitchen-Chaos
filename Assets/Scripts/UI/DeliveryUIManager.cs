using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class DeliveryUIManager : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform recipeUITemplate;
        private void Awake()
        {
            recipeUITemplate.gameObject.SetActive(false);
        }
        private void Start()
        {
            DeliveryManager.Instance.OnWaittingRecipeSpawned += DeliveryManager_OnWaittingRecipeSpawned;
            DeliveryManager.Instance.OnWaittingRecipeCompleted += DeliveryManager_OnWaittingRecipeCompleted;
        }

        private void DeliveryManager_OnWaittingRecipeCompleted(object sender, System.EventArgs e)
        {
            UpdateReciprUI();
        }

        private void DeliveryManager_OnWaittingRecipeSpawned(object sender, System.EventArgs e)
        {
            UpdateReciprUI();
        }


        private void UpdateReciprUI()
        {
            foreach (Transform child in container)
            {
                if (child == recipeUITemplate) continue;
                Destroy(child.gameObject);
            }

            List<RecipeSO> recipeSOList = DeliveryManager.Instance.GetWaittingRecipeList();
            foreach (var recipeSO in recipeSOList)
            {
                Transform recipeUITransform = Instantiate(recipeUITemplate, container);
                recipeUITransform.gameObject.SetActive(true);
                recipeUITransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
            }
        }



    }
}
