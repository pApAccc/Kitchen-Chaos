using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.SavingSystem;
using System.IO;
/// <summary>
/// 
/// </summary>
namespace ns
{
    public class SavingWrapper : MonoBehaviour
    {
        public static SavingWrapper Instance { get; private set; }
        private SavingSystem savingSystem;
        private string path = "save";
        private void Awake()
        {
            Instance = this;
            savingSystem = GetComponent<SavingSystem>();
        }
        private void Start()
        {
            savingSystem.Load(path);
        }
        private void OnApplicationQuit()
        {
            savingSystem.Save(path);
        }

        public void Save()
        {
            savingSystem.Save(path);
        }
    }
}
