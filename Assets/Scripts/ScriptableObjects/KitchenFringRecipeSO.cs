using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    [CreateAssetMenu(menuName = "ScriptableObject/KitchenFringRecipeSO")]
    public class KitchenFringRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public float cookingTimeMax;
    }
}
