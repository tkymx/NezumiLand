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

        public List<PlayerOnegaiEntry> PlayerOnegaiEntrys { get; private set; }
        public List<PlayerEventEntry> PlayerEventEntrys { get; private set; }
        public List<PlayerMonoInfoEntry> PlayerMonoInfoEntrys { get; private set; }
        public List<PlayerMouseStockEntry> PlayerMouseStockEntrys { get; private set; }
        public List<PlayerMonoViewEntry> PlayerMonoViewEntrys { get; private set; }
        public List<PlayerArrangementTargetEntry> PlayerArrangementTargetEntrys { get; private set; }
        public List<PlayerMouseViewEntry> PlayerMouseViewEntrys { get; private set; }
        public List<PlayerInfoEntry> PlayerInfoEntrys { get; private set; }
        public List<PlayerAppearCharacterReserveEntry> PlayerAppearCharacterReserveEntrys { get; private set; }
        public List<PlayerAppearCharacterViewEntry> PlayerAppearCharacterViewEntrys { get; private set; }
        public List<PlayerEarnCurrencyEntry> PlayerEarnCurrencyEntrys { get; private set; }
        public List<PlayerAppearConversationCharacterDirectorEntry> PlayerAppearConversationCharacterDirectorEntrys { get; private set; }
        public List<PlayerAppearOnegaiCharacterDirectorEntry> PlayerAppearOnegaiCharacterDirectorEntrys { get; private set; }
        public List<PlayerAppearPlayingCharacterDirectorEntry> PlayerAppearPlayingCharacterDirectorEntrys { get; private set; }

        public static void Initialize () {
            defaultMap = new PlayerContextMap ();
            defaultMap.Load ();
        }

        public void Load () {
            this.PlayerOnegaiEntrys = LoadEntryFromJsonFromName<PlayerOnegaiEntry> ("PlayerOnegaiEntry").ToList();
            this.PlayerEventEntrys = LoadEntryFromJsonFromName<PlayerEventEntry> ("PlayerEventEntry").ToList();
            this.PlayerMonoInfoEntrys = LoadEntryFromJsonFromName<PlayerMonoInfoEntry> ("PlayerMonoInfoEntry").ToList();
            this.PlayerMouseStockEntrys = LoadEntryFromJsonFromName<PlayerMouseStockEntry> ("PlayerMouseStockEntry").ToList();
            this.PlayerMonoViewEntrys = LoadEntryFromJsonFromName<PlayerMonoViewEntry> ("PlayerMonoViewEntry").ToList();
            this.PlayerArrangementTargetEntrys = LoadEntryFromJsonFromName<PlayerArrangementTargetEntry> ("PlayerArrangementTargetEntry").ToList();
            this.PlayerMouseViewEntrys = LoadEntryFromJsonFromName<PlayerMouseViewEntry> ("PlayerMouseViewEntry").ToList();
            this.PlayerInfoEntrys = LoadEntryFromJsonFromName<PlayerInfoEntry> ("PlayerInfoEntry").ToList();
            this.PlayerAppearCharacterReserveEntrys = LoadEntryFromJsonFromName<PlayerAppearCharacterReserveEntry> ("PlayerAppearCharacterReserveEntry").ToList();
            this.PlayerAppearCharacterViewEntrys = LoadEntryFromJsonFromName<PlayerAppearCharacterViewEntry> ("PlayerAppearCharacterViewEntry").ToList();
            this.PlayerEarnCurrencyEntrys = LoadEntryFromJsonFromName<PlayerEarnCurrencyEntry> ("PlayerEarnCurrencyEntry").ToList();
            this.PlayerAppearConversationCharacterDirectorEntrys = LoadEntryFromJsonFromName<PlayerAppearConversationCharacterDirectorEntry> ("PlayerAppearConversationCharacterDirectorEntry").ToList();
            this.PlayerAppearOnegaiCharacterDirectorEntrys = LoadEntryFromJsonFromName<PlayerAppearOnegaiCharacterDirectorEntry> ("PlayerAppearOnegaiCharacterDirectorEntry").ToList();
            this.PlayerAppearPlayingCharacterDirectorEntrys = LoadEntryFromJsonFromName<PlayerAppearPlayingCharacterDirectorEntry> ("PlayerAppearPlayingCharacterDirectorEntry").ToList();
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