using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class DeliveryManagerSingleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private Transform container;
        [SerializeField] private Transform iconTemplate;
        private void Awake()
        {
            iconTemplate.gameObject.SetActive(false);
        }
        public void SetRecipeSO(RecipeSO recipeSO)
        {
            recipeNameText.text = recipeSO.recipeName;

            List<KitchenObjectSO> kitchenObjectList = recipeSO.kitchenObjectList;
            foreach (KitchenObjectSO kitchenObjectSO in kitchenObjectList)
            {
                Transform iconTransform = Instantiate(iconTemplate, container);
                iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
                iconTransform.gameObject.SetActive(true);
            }
        }

    }
}
