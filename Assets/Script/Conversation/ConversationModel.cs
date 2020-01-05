using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public class ConversationModel: ModelBase {
        public List<string> ConversationTexts { get; private set; }
        public List<string> ConversationCharacterNames { get; private set; }
        public List<string> ConversationCharacterDisplayNames { get; private set; }

        public ConversationModel (
            uint id,
            List<string> conversationTexts,
            List<string> conversationCharacterNames,
            List<string> conversationCharacterDisplayNames
            ) {
            this.Id = id;
            this.ConversationTexts = conversationTexts;
            this.ConversationCharacterNames = conversationCharacterNames;
            this.ConversationCharacterDisplayNames = conversationCharacterDisplayNames;
        }

        public bool IsLastIndex(int index) {
            return this.ConversationTexts.Count-1 == index;
        }
    }
}
