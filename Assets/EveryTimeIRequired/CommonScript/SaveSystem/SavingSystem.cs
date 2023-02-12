using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.SavingSystem
{
    public class SavingSystem : MonoBehaviour
    {
        private string lastSceneBuildIndex = "lastSceneBuildIndex";
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey(lastSceneBuildIndex))
            {
                buildIndex = (int)state[lastSceneBuildIndex];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        /// <summary>
        /// 返回读取的文件内容
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将存贮的内容写入文件
        /// </summary>
        /// <param name="saveFile"></param>
        /// <param name="state"></param>
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        /// <summary>
        /// 存贮所有拥有SaveableEntity物体的状态
        /// </summary>
        /// <param name="state"></param>
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            state[lastSceneBuildIndex] = SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// 恢复所有拥有SaveableEntity物体的状态
        /// </summary>
        /// <param name="state"></param>
        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                //避免文件不存在时加载
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        /// <summary>
        /// 获得存贮路径
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}