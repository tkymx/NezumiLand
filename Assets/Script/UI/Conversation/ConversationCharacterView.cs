using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class ConversationCharacterView : MonoBehaviour
    {
        [SerializeField]
        private Image characterImage = null;

        public void UpdateCharacterImage(string characterName) {
            var sprite = ResourceLoader.LoadCharacterSprite(characterName);
            characterImage.sprite = sprite;
        }
    }
}

