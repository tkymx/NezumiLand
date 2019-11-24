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

        public static void Initialize () {
            defaultMap = new PlayerContextMap ();
            defaultMap.Load ();
        }

        public void Load () {
            this.PlayerOnegaiEntrys = LoadEntryFromJson<PlayerOnegaiEntry> (ResourceLoader.LoadPlayerEntry ("PlayerOnegaiEntry"));
            this.PlayerEventEntrys = LoadEntryFromJson<PlayerEventEntry> (ResourceLoader.LoadPlayerEntry ("PlayerEventEntry"));
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