using UnityEngine;
using System.Collections;
using System.IO;

namespace NL
{
    public class ResourceLoader
    {
        public static string LoadPlayerEntry(string name)
        {
            string path = Application.dataPath + "/PlayerData/" + name + ".json";
            Debug.Assert(File.Exists(path),"ファイルが存在しません : " + path);
            string json = File.ReadAllText(path);
            return json;
        }

        public static void WritePlayerEntry(string name, string json)
        {
            string path = Application.dataPath + "/PlayerData/" + name + ".json";
            Debug.Assert(File.Exists(path),"ファイルが存在しません : " + path);
            File.WriteAllText(path, json);
        }        

        public static string LoadText(string path)
        {
            TextAsset text = Resources.Load<TextAsset>(path);
            if (text == null)
            {
                Debug.LogError(path+"が存在しません。");
            }
            return text.text;
        }
        public static GameObject LoadPrefab(string path)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            if (prefab == null)
            {
                Debug.LogError(path+"が存在しません。");
            }
            return prefab;
        }

        public static string LoadData(string modelName)
        {
            return LoadText("Data/" + modelName);
        }

        public static GameObject LoadModel(string modelName)
        {
            return LoadPrefab("Model/"+modelName);
        }
    }
}
