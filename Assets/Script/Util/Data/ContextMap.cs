using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
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

        public IList<MonoInfoEntry> MonoInfoEntrys { get; private set; }
        public IList<OnegaiEntry> OnegaiEntrys { get; private set; }
        public IList<EventConditionEntry> EventConditionEntrys { get; private set; }
        public IList<EventContentsEntry> EventContentsEntrys { get; private set; }
        public IList<EventEntry> EventEntrys { get; private set; }
        public IList<ConversationEntry> ConversationEntrys { get; private set; }
        public IList<RewardEntry> RewardEntrys { get; private set; }

        public static void Initialize () {
            defaultMap = new ContextMap ();
            defaultMap.Load ();
        }

        public void Load () {
            this.MonoInfoEntrys = LoadEntryFromJson<MonoInfoEntry> (ResourceLoader.LoadData ("MonoInfoEntry"));
            this.OnegaiEntrys = LoadEntryFromJson<OnegaiEntry> (ResourceLoader.LoadData ("OnegaiEntry"));
            this.EventConditionEntrys = LoadEntryFromJson<EventConditionEntry> (ResourceLoader.LoadData ("EventConditionEntry"));
            this.EventContentsEntrys = LoadEntryFromJson<EventContentsEntry> (ResourceLoader.LoadData ("EventContentsEntry"));
            this.EventEntrys = LoadEntryFromJson<EventEntry> (ResourceLoader.LoadData ("EventEntry"));
            this.ConversationEntrys = LoadEntryFromJson<ConversationEntry> (ResourceLoader.LoadData ("ConversationEntry"));
            this.RewardEntrys = LoadEntryFromJson<RewardEntry> (ResourceLoader.LoadData ("RewardEntry"));
        }

        private static IList<T> LoadEntryFromJson<T> (string json) {
            using (var stream = new MemoryStream (Encoding.UTF8.GetBytes (json))) {
                var serializer = new DataContractJsonSerializer (typeof (IList<T>));
                return (IList<T>) serializer.ReadObject (stream);
            }
        }
    }

}