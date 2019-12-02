using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator : MonoBehaviour
    {
        private AppearCharacterModel appearCharacterModel;
        private ConversationModel conversationModel;
        private AppearCharacterViewModel generatedAppearCharacterViewModel;

        public AppearCharacterGenerator(AppearCharacterModel appearCharacterModel, ConversationModel conversationModel)
        {
            this.appearCharacterModel = appearCharacterModel;
            this.conversationModel = conversationModel;
            this.generatedAppearCharacterViewModel = null;
        }

        public AppearCharacterViewModel Generate() {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , GetInitialPosition());
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            this.generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                appearCharacterModel,
                conversationModel
            );
            return generatedAppearCharacterViewModel;
        }

        private static Vector3 GetInitialPosition() 
        {
            return new Vector3(32,5,0);
        }

        public override string ToString() {
            return "AppearCharacterGenerator" + appearCharacterModel.Name + " " + conversationModel.Id.ToString();
        }

        public bool IsTarget(AppearCharacterViewModel appearCharacterViewModel) {
            if (this.generatedAppearCharacterViewModel == null) {
                return false;
            }
            return this.generatedAppearCharacterViewModel == appearCharacterViewModel;
        }
    }
}
