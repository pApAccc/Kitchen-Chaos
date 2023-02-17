using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    [CreateAssetMenu(menuName = "ScriptableObject/KitchenBuringRecipeSO")]
    public class KitchenBuringRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public float buringTimeMax;
    }
}
