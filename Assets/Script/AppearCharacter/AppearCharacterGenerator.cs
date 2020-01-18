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
        private PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel;

        public AppearCharacterGenerator(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            this.appearCharacterModel = playerAppearCharacterReserveModel.AppearCharacterModel;
            this.conversationModel = playerAppearCharacterReserveModel.ConversationModel;
            this.rewardModel = playerAppearCharacterReserveModel.RewardModel;
            this.playerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
        }

        public AppearCharacterViewModel Generate() {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , GetInitialPosition());
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                GameManager.Instance.AppearCharacterManager.Create(appearCharacterView.transform, playerAppearCharacterReserveModel)
            );
            generatedAppearCharacterViewModel.SetInitialState();
            return generatedAppearCharacterViewModel;
        }

        public AppearCharacterViewModel Generate(PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , playerAppearCharacterViewModel.Position);
            appearCharacterInstance.transform.rotation = Quaternion.Euler(playerAppearCharacterViewModel.Rotation);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                playerAppearCharacterViewModel
            );
            generatedAppearCharacterViewModel.InterruptState(playerAppearCharacterViewModel.AppearCharacterState);
            return generatedAppearCharacterViewModel;
        }        

        private static Vector3 GetInitialPosition() 
        {
            return new Vector3(Random.Range(-32,32),0,Random.Range(-32,32));
        }

        public override string ToString() {
            return "AppearCharacterGenerator" + appearCharacterModel.Name + " " + conversationModel.Id.ToString();
        }
    }
}
