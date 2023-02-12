using RPG.Saving;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 存储系统包装器示例
/// </summary>
namespace ns.SceneMangement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        private SavingSystem savingSystem;

        [SerializeField] private float fadeInTime = 1f;
        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }
        private IEnumerator Start()
        {
            //可以添加一个淡入效果
            yield return savingSystem.LoadLastScene(defaultSaveFile);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Save();
            }
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }
}
