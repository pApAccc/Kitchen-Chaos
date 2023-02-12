using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

///<summary>
///资源管理器:读取ConfigMap.txt中的数据 -->  资源名,资源路径  
///ConfigMap.txt目前保存了所有预制件的名称与路径
///</summary>
namespace Common
{
    public static class ResourceManager
    {
        private static Dictionary<string, string> configMap;

        //初始化类的静态数据成员
        static ResourceManager()
        {
            configMap = new Dictionary<string, string>();
            string fileContent = ConfigruationReader.GetConfigFile("ConfigMap.txt");
            ConfigruationReader.Reader(fileContent, (string line) =>
            {
                //分割字符串
                string[] keyValue = line.Split('=');
                //加入字典
                configMap.Add(keyValue[0], keyValue[1]);
            });
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="prefabName">预制件名称</param>
        /// <returns>预制件</returns>
        public static T Load<T>(string prefabName) where T : Object
        {
            //将perfabName转换为prefabPath
            string prefabPath = configMap[prefabName];
            return Resources.Load<T>(prefabPath);
        }

    }
}
