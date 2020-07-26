using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterGenerator
    {
        public AppearCharacterGenerator()
        {
        }

        public AppearCharacterViewModel GenerateFromReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {

            // 初期座標
            MovePath movePath = new MovePath(GetInitialPosition(), GetInitialPosition() );

            var modelPrefab = ResourceLoader.LoadModel(playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase.AppearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , movePath.AppearPosition);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();

            // ViewModel 用のプレイヤーデータを作成
            var playerAppearCharacterViewModel = GameManager.Instance.AppearCharacterManager.Create(
                appearCharacterView.transform, 
                movePath,
                playerAppearCharacterReserveModel.AppearCharacterLifeDirectorType,
                playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase,
                playerAppearCharacterReserveModel);

            // ViewModel の作成
            var generatedAppearCharacterViewModel = new AppearCharacterViewModel(
                appearCharacterView,
                playerAppearCharacterViewModel
            );
            return generatedAppearCharacterViewModel;
        }     

        public AppearCharacterViewModel Generate(PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            var modelPrefab = ResourceLoader.LoadModel(playerAppearCharacterViewModel.AppearCharacterModel.Name);
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
    }
}
