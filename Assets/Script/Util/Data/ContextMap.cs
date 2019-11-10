using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using UnityEngine;

namespace NL
{
    public class ContextMap
    {
        private static ContextMap defaultMap = null;
        public static ContextMap DefaultMap {
            get
            {
                Debug.Assert(defaultMap != null, "DefaultMapが初期枯れておりません。");
                return defaultMap;
            }
        }

        public IList<MonoInfoEntry> MonoInfoEntrys { get; private set; }
        public IList<OnegaiEntry> OnegaiEntrys { get; private set; }

        public static void Initialize()
        {
            defaultMap = new ContextMap();
            defaultMap.Load();
        }

        public void Load()
        {
            this.MonoInfoEntrys = LoadEntryFromJson<MonoInfoEntry>(ResourceLoader.LoadData("MonoInfoEntry"));
            this.OnegaiEntrys = LoadEntryFromJson<OnegaiEntry>(ResourceLoader.LoadData("OnegaiEntry"));
        }

        private static IList<T> LoadEntryFromJson<T>(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(IList<T>));
                return (IList<T>)serializer.ReadObject(stream);
            }
        }
    }

}