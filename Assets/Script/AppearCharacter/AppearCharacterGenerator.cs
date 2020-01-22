using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator : MonoBehaviour
    {
        private AppearCharacterModel appearCharacterModel;

        public AppearCharacterGenerator(AppearCharacterModel appearCharacterModel)
        {
            this.appearCharacterModel = appearCharacterModel;
        }

        public AppearCharacterViewModel GenerateReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , GetInitialPosition());
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                GameManager.Instance.AppearCharacterManager.Create(appearCharacterView.transform, appearCharacterModel, playerAppearCharacterReserveModel, AppearCharacterLifeDirectorType.Reserve)
            );
            generatedAppearCharacterViewModel.SetInitialState();
            return generatedAppearCharacterViewModel;
        }

        public AppearCharacterViewModel GenerateParkOpen(Vector3 appearPosition) {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , appearPosition);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                GameManager.Instance.AppearCharacterManager.Create(appearCharacterView.transform, appearCharacterModel, null, AppearCharacterLifeDirectorType.ParkOpen)
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
            return "AppearCharacterGenerator" + appearCharacterModel.Name;
        }
    }
}
