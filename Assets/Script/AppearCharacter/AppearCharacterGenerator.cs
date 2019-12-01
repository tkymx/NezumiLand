using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator : MonoBehaviour
    {
        public static AppearCharacterViewModel Generate(AppearCharacterModel appearCharacterModel, ConversationModel conversationModel) {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , GetInitialPosition());
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            return new AppearCharacterViewModel(
                appearCharacterView,
                appearCharacterModel,
                conversationModel
            );
        }

        private static Vector3 GetInitialPosition() 
        {
            return new Vector3(5,5,0);
        }
    }
}
