using System.Collections;
using System.IO;
using UnityEngine;

namespace NL {
    public class ResourceLoader {
        public static bool ExistsPlayerEntry (string name) {
            string path = PlayerDataPath(name);
            return File.Exists (path);
        }
        public static string LoadPlayerEntry (string name) {
            string path = PlayerDataPath(name);
            Debug.Assert (File.Exists (path), "ファイルが存在しません : " + path);
            string json = File.ReadAllText (path);
            return json;
        }

        public static void WritePlayerEntry (string name, string json) {
            string path = PlayerDataPath(name);
            if (!Directory.Exists(PlayerDataDirPath())) {
                Directory.CreateDirectory(PlayerDataDirPath());
            }
            File.WriteAllText (path, json);
        }

        private static string PlayerDataDirPath () {
#if UNITY_EDITOR
            return Application.dataPath + "/PlayerData/";
#else
            return Application.persistentDataPath + "/PlayerData/";
#endif            
        }

        private static string PlayerDataPath (string name) {
            return PlayerDataDirPath () + "/" + name + ".json";
        }

        public static void RemoveAllPlayerData () {
            Directory.Delete(PlayerDataDirPath(),true);
        }

        public static string LoadText (string path) {
            TextAsset text = Resources.Load<TextAsset> (path);
            if (text == null) {
                Debug.LogError (path + "が存在しません。");
            }
            return text.text;
        }
        public static GameObject LoadPrefab (string path) {
            GameObject prefab = (GameObject) Resources.Load (path);
            if (prefab == null) {
                Debug.LogError (path + "が存在しません。");
            }
            return prefab;
        }

        public static Sprite LoadCharacterSprite (string characterName) {
            var path = "CharacterImage/" + characterName;
            Sprite sprite = Resources.Load<Sprite> (path);
            if (sprite == null) {
                Debug.LogError (path + "が存在しません。");
            }
            return sprite;
        }

        public static Sprite LoadItemSprite (string characterName) {
            var path = "ItemImage/" + characterName;
            Sprite sprite = Resources.Load<Sprite> (path);
            if (sprite == null) {
                Debug.LogError (path + "が存在しません。");
            }
            return sprite;
        }          

        public static Sprite LoadScreenSprite (string imageName) {
            var path = "Screen/" + imageName;
            Sprite sprite = Resources.Load<Sprite> (path);
            if (sprite == null) {
                Debug.LogError (path + "が存在しません。");
            }
            return sprite;
        }          


        public static string LoadData (string modelName) {
            return LoadText ("Data/" + modelName);
        }

        public static GameObject LoadModel (string modelName) {
            return LoadPrefab ("Model/" + modelName);
        }
    }
}