using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    [CreateAssetMenu(menuName = "ScriptableObject/RecipeSO")]
    public class RecipeSO : ScriptableObject
    {
        public List<KitchenObjectSO> kitchenObjectList;
        public string recipeName;
    }
}
