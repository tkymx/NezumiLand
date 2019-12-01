using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class AppearCharacterEntry {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    public interface IAppearCharacterRepository {
        IEnumerable<AppearCharacterModel> GetAll ();
        AppearCharacterModel Get (uint id);
    }

    public class AppearCharacterRepository : RepositoryBase<AppearCharacterEntry>, IAppearCharacterRepository {
        public AppearCharacterRepository (ContextMap contextMap) : base (contextMap.AppearCharacterEntrys) { }

        public IEnumerable<AppearCharacterModel> GetAll () {
            return entrys.Select (entry => {
                return new AppearCharacterModel (
                    entry.Id,
                    entry.Name);
            });
        }

        public AppearCharacterModel Get (uint id) {
            var entry = this.entrys.Where (e => e.Id == id).First ();
            Debug.Assert (entry != null, "ファイルが見つかりません : " + id.ToString ());
            return new AppearCharacterModel (
                entry.Id,
                entry.Name);
        }
    }
}