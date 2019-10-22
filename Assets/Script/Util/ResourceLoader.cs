using UnityEngine;
using System.Collections;

namespace NL
{
    public class ResourceLoader
    {
        public static GameObject LoadPrefab(string path)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            if (prefab == null)
            {
                Debug.LogError(path+"が存在しません。");
            }
            return prefab;
        }
    }
}
