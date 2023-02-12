using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

///<summary>
///通用文件读取器
///</summary>
namespace Common
{
    public class ConfigruationReader
    {
        /// <summary>
        /// 根据文件名查找文件内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件内容</returns>
        public static string GetConfigFile(string fileName)
        {
            //StreamingAssets在部分平台会出错
            //string url = "file://" + Application.streamingAssetsPath + "/fileName";
            string url;

            //unity宏标签
#if UNITY_EDITOR || UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
#elif UNITY_IPHONE
            url = "file://" + Application.dataPath + "/Raw/"+ fileName";
#elif UNITY_ANDROID
            url = "jar:file://" + Application.dataPath + "!/assets/" + fileName";
#endif
            //加载资源
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SendWebRequest();
            while (true)
            {
                if (request == null) return null;
                if (request.downloadHandler.isDone)
                {
                    return request.downloadHandler.text;
                }
            }
        }

        /// <summary>
        /// 根据文件内容进行读取
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="handle">字符串解析方法</param>
        public static void Reader(string fileContent, Action<string> handle)
        {
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    handle(line);

                }
            }
        }
    }
}
