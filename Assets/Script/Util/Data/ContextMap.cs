using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;
using UnityEngine;

namespace NL {
    public class ContextMap {
        private static ContextMap defaultMap = null;
        public static ContextMap DefaultMap {
            get {
                Debug.Assert (defaultMap != null, "DefaultMapが初期枯れておりません。");
                return defaultMap;
            }
        }

        public List<MonoInfoEntry> MonoInfoEntrys { get; private set; }
        public List<OnegaiEntry> OnegaiEntrys { get; private set; }
        public List<EventConditionEntry> EventConditionEntrys { get; private set; }
        public List<EventContentsEntry> EventContentsEntrys { get; private set; }
        public List<EventEntry> EventEntrys { get; private set; }
        public List<ConversationEntry> ConversationEntrys { get; private set; }
        public List<RewardEntry> RewardEntrys { get; private set; }
        public List<AppearCharacterEntry> AppearCharacterEntrys { get; private set; }
        public List<MousePurchaceTableEntry> MousePurchaceTableEntrys { get; private set; }
        public List<ScheduleEntry> ScheduleEntrys { get; private set; }

        public static void Initialize () {
            defaultMap = new ContextMap ();
            defaultMap.Load ();
        }

        [System.Serializable]
        public class Lap<T> {
            public T[] InnerArray;
        }

        public void Load () {
            this.MonoInfoEntrys = LoadEntryFromJson<MonoInfoEntry> (ResourceLoader.LoadData ("MonoInfoEntry")).ToList();
            this.OnegaiEntrys = LoadEntryFromJson<OnegaiEntry> (ResourceLoader.LoadData ("OnegaiEntry")).ToList();
            this.EventConditionEntrys = LoadEntryFromJson<EventConditionEntry> (ResourceLoader.LoadData ("EventConditionEntry")).ToList();
            this.EventContentsEntrys = LoadEntryFromJson<EventContentsEntry> (ResourceLoader.LoadData ("EventContentsEntry")).ToList();
            this.EventEntrys = LoadEntryFromJson<EventEntry> (ResourceLoader.LoadData ("EventEntry")).ToList();
            this.ConversationEntrys = LoadEntryFromJson<ConversationEntry> (ResourceLoader.LoadData ("ConversationEntry")).ToList();
            this.RewardEntrys = LoadEntryFromJson<RewardEntry> (ResourceLoader.LoadData ("RewardEntry")).ToList();
            this.AppearCharacterEntrys = LoadEntryFromJson<AppearCharacterEntry> (ResourceLoader.LoadData ("AppearCharacterEntry")).ToList();
            this.MousePurchaceTableEntrys = LoadEntryFromJson<MousePurchaceTableEntry> (ResourceLoader.LoadData ("MousePurchaceTableEntry")).ToList();
            this.ScheduleEntrys = LoadEntryFromJson<ScheduleEntry> (ResourceLoader.LoadData ("ScheduleEntry")).ToList();
        }

        private static T[] LoadEntryFromJson<T> (string json) {
            using (var stream = new MemoryStream (Encoding.UTF8.GetBytes (json))) {
                var lap = JsonUtility.FromJson<Lap<T>>(json);
                return lap.InnerArray;
            }
        }
    }

}