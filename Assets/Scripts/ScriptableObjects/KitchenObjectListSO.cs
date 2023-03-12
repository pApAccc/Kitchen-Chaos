using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    //[CreateAssetMenu(fileName = "KitchenObjectListSO", menuName = "ScriptableObject/KitchenObjectListSO")]
    public class KitchenObjectListSO : ScriptableObject
    {
        public List<KitchenObjectSO> kitchenObjectSOList;
    }
}
