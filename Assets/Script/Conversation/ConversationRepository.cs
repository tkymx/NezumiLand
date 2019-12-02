using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class ConversationEntry : EntryBase {

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
            var entry = base.GetEntry(id);
            return new ConversationModel (
                entry.Id,
                entry.ConversationTexts.ToList(),
                entry.ConversationCharacterNames.ToList());
        }
    }
}