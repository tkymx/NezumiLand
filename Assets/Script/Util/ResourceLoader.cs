using UnityEngine;
using System.Collections;

namespace NL
{
    public class ResourceLoader
    {
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
