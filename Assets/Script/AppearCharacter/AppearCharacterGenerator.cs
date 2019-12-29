using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator : MonoBehaviour
    {
        private AppearCharacterModel appearCharacterModel;
        private ConversationModel conversationModel;
        private RewardModel rewardModel;
        private AppearCharacterViewModel generatedAppearCharacterViewModel;

        public AppearCharacterGenerator(AppearCharacterModel appearCharacterModel, ConversationModel conversationModel,RewardModel rewardModel)
        {
            this.appearCharacterModel = appearCharacterModel;
            this.conversationModel = conversationModel;
            this.rewardModel = rewardModel;

            this.generatedAppearCharacterViewModel = null;
        }

        public AppearCharacterViewModel Generate() {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , GetInitialPosition());
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            this.generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                this.appearCharacterModel,
                this.conversationModel,
                this.rewardModel
            );
            return generatedAppearCharacterViewModel;
        }

        private static Vector3 GetInitialPosition() 
        {
            return new Vector3(Random.Range(-32,32),0,Random.Range(-32,32));
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
