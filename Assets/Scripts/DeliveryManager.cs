using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class DeliveryManager : MonoBehaviour
    {
        public event EventHandler OnWaittingRecipeSpawned;
        public event EventHandler OnWaittingRecipeCompleted;
        public event EventHandler OnWaittingRecipeSuccessed;
        public event EventHandler OnWaittingRecipeFailed;

        public static DeliveryManager Instance;

        [SerializeField] private RecipeListSO recipeListSO;

        private List<RecipeSO> waittingRecipeList;
        private float recipeTimer;
        private float recipeTimerMax = 4f;
        private int waittingRecipeMax = 4;
        private int recipeHasDeliveredAmount;
        private void Awake()
        {
            Instance = this;
            waittingRecipeList = new List<RecipeSO>();
            recipeTimer = recipeTimerMax;
        }
        private void Update()
        {
            if (waittingRecipeList.Count == waittingRecipeMax) return;

            recipeTimer -= Time.deltaTime;
            if (recipeTimer < 0)
            {
                recipeTimer = recipeTimerMax;
                RecipeSO recipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                waittingRecipeList.Add(recipeSO);

                OnWaittingRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DeliverRecipe(PlateObject plateObject)
        {
            for (int i = 0; i < waittingRecipeList.Count; i++)
            {
                //个数不一致，skip
                if (waittingRecipeList[i].kitchenObjectList.Count != plateObject.GetKitchenObjectSOList().Count)
                    continue;

                bool isMatch = true;
                foreach (KitchenObjectSO kitchenObjectSO in waittingRecipeList[i].kitchenObjectList)
                {
                    if (!plateObject.GetKitchenObjectSOList().Contains(kitchenObjectSO))
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    recipeHasDeliveredAmount++;
                    waittingRecipeList.RemoveAt(i);
                    OnWaittingRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnWaittingRecipeSuccessed?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
            OnWaittingRecipeFailed?.Invoke(this, EventArgs.Empty);
        }

        public List<RecipeSO> GetWaittingRecipeList()
        {
            return waittingRecipeList;
        }

        public int GetRecipesHasDeliveredAmount() => recipeHasDeliveredAmount;
    }
}