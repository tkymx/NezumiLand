using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
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

        private static IList<T> LoadEntryFromJsonFromName<T> (string name) {
            if (!ResourceLoader.ExistsPlayerEntry(name)) {
                return new List<T>();
            }
            return LoadEntryFromJson<T>(ResourceLoader.LoadPlayerEntry (name));
        }

        private static IList<T> LoadEntryFromJson<T> (string json) {
            using (var stream = new MemoryStream (Encoding.UTF8.GetBytes (json))) {
                var serializer = new DataContractJsonSerializer (typeof (IList<T>));
                return (IList<T>) serializer.ReadObject (stream);
            }
        }

        public static void WriteEntry<T> (IList<T> entrys) {
            using (var stream = new MemoryStream ()) {
                var serializer = new DataContractJsonSerializer (typeof (IList<T>));
                serializer.WriteObject (stream, entrys);
                Debug.Log (typeof (T).Name + "の書き込みをします");
                ResourceLoader.WritePlayerEntry (typeof (T).Name, Encoding.UTF8.GetString (stream.GetBuffer (), 0, (int) stream.Length));
            }
        }
    }

}