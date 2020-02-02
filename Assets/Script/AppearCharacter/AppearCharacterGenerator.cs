using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator
    {
        private AppearCharacterModel appearCharacterModel;

        public AppearCharacterGenerator(AppearCharacterModel appearCharacterModel)
        {
            this.appearCharacterModel = appearCharacterModel;
        }

        public AppearCharacterViewModel GenerateReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            //TODO:　予約時の座標のとり方正常化
            MovePath movePath = new MovePath(GetInitialPosition(), GetInitialPosition() );

            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , movePath.AppearPosition);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                GameManager.Instance.AppearCharacterManager.Create(appearCharacterView.transform, appearCharacterModel, AppearCharacterLifeDirectorType.Reserve, playerAppearCharacterReserveModel, movePath)
            );
            generatedAppearCharacterViewModel.SetInitialState();
            return generatedAppearCharacterViewModel;
        }

        public AppearCharacterViewModel GenerateParkOpen(MovePath movePath) {
            var modelPrefab = ResourceLoader.LoadModel(appearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , movePath.AppearPosition);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                GameManager.Instance.AppearCharacterManager.Create(appearCharacterView.transform, appearCharacterModel, AppearCharacterLifeDirectorType.ParkOpen, null, movePath)
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
