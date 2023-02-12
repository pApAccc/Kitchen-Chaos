using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///AIConfigurationReader配置工厂
///</summary>
namespace FSM
{
    public class AIConfigurationReaderFactory
    {
        private static Dictionary<string, AIConfigurationReader> cache;
        static AIConfigurationReaderFactory()
        {
            cache = new Dictionary<string, AIConfigurationReader>();
        }
        public static Dictionary<string, Dictionary<string, string>> GetMap(string path)
        {
            if (!cache.ContainsKey(path)) { cache.Add(path, new AIConfigurationReader(path)); }
            return cache[path].Map;
        }
    }
}
