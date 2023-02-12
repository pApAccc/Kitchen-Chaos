using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 所有需要频繁创建销毁的物体，交由对象池创建回收
需要通过对象池创建的物体，如需每次创建,启用时执行，交由IResetable
 */
///<summary>
///对象池
///</summary>
namespace Common
{
    //使用接口代替Start和Awake
    public interface IResetable
    {
        void OnReset();
    }
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        //对象池集合
        private Dictionary<string, List<GameObject>> cache;
        protected override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// 创捷对象
        /// </summary>
        /// <param name="key">对象类型</param>
        /// <param name="prefab">对象预制件</param>
        /// <param name="pos">创建的位置</param>
        /// <param name="rotate">创建的角度</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate)
        {
            GameObject go = null;

            if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
            //查找未被禁用的物体
            go = cache[key].Find(go => !go.activeInHierarchy);

            if (go == null)
            {
                go = Instantiate(prefab);
                cache[key].Add(go);
            }

            //设置go属性
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            //将需要重设的数据放在start awake中只会在对象创建后执行一次，所以放在接口中调用
            //遍历物体身上所有需要重置的逻辑
            foreach (var item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            }
            return go;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">被回收的对象</param>
        /// <param name="delay">延迟时间,默认为</param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            if (delay == 0)
                go.SetActive(false);
            else
                StartCoroutine(CollectDelayObject(go, delay));
        }
        private IEnumerator CollectDelayObject(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

        /// <summary>
        /// 清除健中的数据
        /// </summary>
        /// <param name="key">健</param>
        public void Clear(string key)
        {
            if (cache.ContainsKey(key))
            {
                //因为清除的是引用数据类型，list中存储的是地址，并没有修改list里的数据
                foreach (var item in cache[key])
                {
                    Destroy(item);
                }
                cache.Remove(key);
            }
        }

        /// <summary>
        /// 清空全部
        /// </summary>
        public void ClearAll()
        {
            //内部的Move Next方法会获取下一个元素，但自身被cache.Remove删除了
            //foreach (var item in cache.Keys)
            //{
            //    Clear(item);
            //}

            //遍历list规避问题
            foreach (var item in new List<string>(cache.Keys))
            {
                Clear(item);
            }
        }
    }
}
