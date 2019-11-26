using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class ConversationEntry {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public string[] ConversationTexts { get; set; }

        [DataMember]
        public string[] ConversationCharacterNames { get; set; }
    }

    public interface IConversationRepository {
        IEnumerable<ConversationModel> GetAll ();
        ConversationModel Get (uint id);
    }

    public class ConversationRepository : RepositoryBase<ConversationEntry>, IConversationRepository {
        public ConversationRepository (ContextMap contextMap) : base (contextMap.ConversationEntrys) { }

        public IEnumerable<ConversationModel> GetAll () {
            return entrys.Select (entry => {
                return new ConversationModel (
                    entry.Id,
                    entry.ConversationTexts.ToList(),
                    entry.ConversationCharacterNames.ToList());
            });
        }

        public ConversationModel Get (uint id) {
            var entry = this.entrys.Where (e => e.Id == id).First ();
            Debug.Assert (entry != null, "ConversationModelが見つかりません : " + id.ToString ());
            return new ConversationModel (
                entry.Id,
                entry.ConversationTexts.ToList(),
                entry.ConversationCharacterNames.ToList());
        }
    }
}