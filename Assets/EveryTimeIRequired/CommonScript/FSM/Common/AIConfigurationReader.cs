using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

///<summary>
///读取状态信息
///信息格式为：
///[Idle]
///NoHealth>Dead
///
///[Dead]
///</summary>
namespace FSM
{
    public class AIConfigurationReader
    {
        public Dictionary<string, Dictionary<string, string>> Map;
        string lastLine;

        public AIConfigurationReader(string fileName)
        {
            Map = new Dictionary<string, Dictionary<string, string>>();
            string fileContent = ConfigruationReader.GetConfigFile(fileName);

            ConfigruationReader.Reader(fileContent, (string line) =>
            {
                line = line.Trim();
                //空行
                if (string.IsNullOrEmpty(line)) return;
                if (line.StartsWith('['))
                {
                    line = line.Substring(1, line.Length - 2);
                    //状态
                    Map.Add(line, new Dictionary<string, string>());
                    lastLine = line;
                }
                else
                {
                    string[] keyValue = line.Split('>');
                    //映射
                    Map[lastLine].Add(keyValue[0], keyValue[1]);
                }
            });
        }
    }
}
