using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;
using UnityEngine;

namespace NL {
    public class PlayerContextMap {
        private static PlayerContextMap defaultMap = null;
        public static PlayerContextMap DefaultMap {
            get {
                Debug.Assert (defaultMap != null, "DefaultMapが初期枯れておりません。");
                return defaultMap;
            }
        }

        public IList<PlayerOnegaiEntry> PlayerOnegaiEntrys { get; private set; }
        public IList<PlayerEventEntry> PlayerEventEntrys { get; private set; }
        public IList<PlayerMonoInfoEntry> PlayerMonoInfoEntrys { get; private set; }
        public IList<PlayerMouseStockEntry> PlayerMouseStockEntrys { get; private set; }
        public IList<PlayerMonoViewEntry> PlayerMonoViewEntrys { get; private set; }
        public IList<PlayerArrangementTargetEntry> PlayerArrangementTargetEntrys { get; private set; }
        public IList<PlayerMouseViewEntry> PlayerMouseViewEntrys { get; private set; }
        public IList<PlayerInfoEntry> PlayerInfoEntrys { get; private set; }

        public static void Initialize () {
            defaultMap = new PlayerContextMap ();
            defaultMap.Load ();
        }

        public void Load () {
            this.PlayerOnegaiEntrys = LoadEntryFromJsonFromName<PlayerOnegaiEntry> ("PlayerOnegaiEntry");
            this.PlayerEventEntrys = LoadEntryFromJsonFromName<PlayerEventEntry> ("PlayerEventEntry");
            this.PlayerMonoInfoEntrys = LoadEntryFromJsonFromName<PlayerMonoInfoEntry> ("PlayerMonoInfoEntry");
            this.PlayerMouseStockEntrys = LoadEntryFromJsonFromName<PlayerMouseStockEntry> ("PlayerMouseStockEntry");
            this.PlayerMonoViewEntrys = LoadEntryFromJsonFromName<PlayerMonoViewEntry> ("PlayerMonoViewEntry");
            this.PlayerArrangementTargetEntrys = LoadEntryFromJsonFromName<PlayerArrangementTargetEntry> ("PlayerArrangementTargetEntry");
            this.PlayerMouseViewEntrys = LoadEntryFromJsonFromName<PlayerMouseViewEntry> ("PlayerMouseViewEntry");
            this.PlayerInfoEntrys = LoadEntryFromJsonFromName<PlayerInfoEntry> ("PlayerInfoEntry");
        }

        [System.Serializable]
        public class PlayerLap<T> {
            public T[] InnerArray;

            public PlayerLap(IList<T> innerArray)
            {
                this.InnerArray = innerArray.ToArray();
            }
        }

        private static IList<T> LoadEntryFromJsonFromName<T> (string name) {
            if (!ResourceLoader.ExistsPlayerEntry(name)) {
                return new List<T>();
            }
            return LoadEntryFromJson<T>(ResourceLoader.LoadPlayerEntry (name));
        }

        private static T[] LoadEntryFromJson<T> (string json) {
            using (var stream = new MemoryStream (Encoding.UTF8.GetBytes (json))) {
                var lap = JsonUtility.FromJson<PlayerLap<T>>(json);
                return lap.InnerArray;
            }
        }

        public static void WriteEntry<T> (IList<T> entrys) {
            using (var stream = new MemoryStream ()) {
                var lap = new PlayerLap<T>(entrys);
                var json = JsonUtility.ToJson(lap);
                Debug.Log (typeof (T).Name + "の書き込みをします");
                ResourceLoader.WritePlayerEntry (typeof (T).Name, json);
            }
        }
    }

}