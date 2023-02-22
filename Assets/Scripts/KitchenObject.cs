using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class KitchenObject : MonoBehaviour
    {
        private IKitchenObjectParent kitchenObjectParent;

        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        public KitchenObjectSO GetKitchenObjectSO()
        {
            return kitchenObjectSO;
        }

        public IKitchenObjectParent GetKitchenObjectParent() => kitchenObjectParent;

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            //设置旧字段
            this.kitchenObjectParent?.ClearKitchenObject();
            this.kitchenObjectParent = kitchenObjectParent;

            //设置新字段
            kitchenObjectParent.SetKitchenObject(this);
            transform.parent = kitchenObjectParent.GetHoldPointTransform();
            transform.localPosition = Vector3.zero;
        }

        public void DestroySelf()
        {
            kitchenObjectParent.ClearKitchenObject();
            Destroy(gameObject);
        }

        /// <summary>
        /// 尝试将KitchenObject转化为PlateObject
        /// </summary>
        /// <param name="plateObject"></param>
        /// <returns></returns>
        public bool TryGetPlate(out PlateObject plateObject)
        {
            if (this is PlateObject)
            {
                plateObject = (PlateObject)this;
                return true;
            }
            else
            {
                plateObject = null;
                return false;
            }
        }

        public static KitchenObject SpwanKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
        {
            Transform kitchenObjectInstance = Instantiate(kitchenObjectSO.prefab);
            KitchenObject kitchenObject = kitchenObjectInstance.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

            return kitchenObject;
        }



    }
}
