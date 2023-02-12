using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
使用：需要特殊目录 StreamingAssets
 
编译器代码:继承自Editor，只需要在unity编译器中执行的代码
AssetDataBase：只适用于编译器中操作相关资源的代码
StreamingAssets：特殊文件之一，该目录中的文件不会被压缩，适合在移动端读取资源（在pc中可以写入）
持久化路径:Application.persistentDataPath 可以在运行时读写,unity外部目录(安装程序时才产生)
 */
///<summary>
///创建资源映射表
///</summary>
public class GenerateResConfig : Editor
{

    [MenuItem("Tools/Resource/Generate ResConfig File")]
    public static void Generate()
    {

        //在"Assets/Resources"下查找
        // FindAssets可以通过t:和l;查找
        string[] resFile = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFile.Length; i++)
        {
            //将GUID替换为路径
            resFile[i] = AssetDatabase.GUIDToAssetPath(resFile[i]);

            //将格式制作为  资源名=资源路径
            string fileName = Path.GetFileNameWithoutExtension(resFile[i]);
            string fileRes = resFile[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", "");
            resFile[i] = fileName + "=" + fileRes;
        }
        //写入文件
        File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", resFile);
        //刷新资源面板
        AssetDatabase.Refresh();
    }
}


