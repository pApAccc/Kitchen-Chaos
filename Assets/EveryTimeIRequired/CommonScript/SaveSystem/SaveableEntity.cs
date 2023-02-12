using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

///<summary>
///对象需要继承ISaveable接口，才能实现存储和读取
///</summary>
namespace ns.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 生成唯一UUID
        /// </summary>
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            //避免自动给预制件设置UUID
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                //设置属性
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;

            #region
            //基本能代替上面，但是操作不能撤销，编辑器不知道你修改了值，无法自动提示值已被修改
            //if (uniqueIdentifier == "" || !IsUnique(uniqueIdentifier))
            //{
            //    uniqueIdentifier = Guid.NewGuid().ToString();
            //    globalLookup[uniqueIdentifier] = this;
            //}
            #endregion
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}