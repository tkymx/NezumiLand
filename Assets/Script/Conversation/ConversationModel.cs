using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public class ConversationModel: ModelBase {
        public List<string> ConversationTexts { get; private set; }
        public List<string> ConversationCharacterNames { get; private set; }

        public ConversationModel (
            uint id,
            List<string> conversationTexts,
            List<string> conversationCharacterNames
            ) {
            this.Id = id;
            this.ConversationTexts = conversationTexts;
            this.ConversationCharacterNames = conversationCharacterNames;
        }

        public bool IsLastIndex(int index) {
            return this.ConversationTexts.Count-1 == index;
        }
    }
}
