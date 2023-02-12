using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    public static class TransformHelper
    {
        /// <summary>
        /// 在子类中按名字查找物体
        /// </summary>
        /// <param name="ParentTF">需要查找的对象</param>
        /// <param name="name">子类名称</param>
        public static Transform FindChildByName(this Transform ParentTF, string name)
        {
            Transform TF = ParentTF.Find(name);
            if (TF != null) return TF;

            for (int i = 0; i < ParentTF.childCount; i++)
            {
                TF = FindChildByName(ParentTF.GetChild(i), name);
                if (TF != null) return TF;
            }

            return null;

        }
    }
}

