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
            //TODO:　予約時の座標のとり方正常化
            MovePath movePath = new MovePath(GetInitialPosition(), GetInitialPosition() );

            var modelPrefab = ResourceLoader.LoadModel(playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase.AppearCharacterModel.Name);
            var appearCharacterInstance = Object.AppearToFloor(modelPrefab, GameManager.Instance.AppearCharacterManager.Root , movePath.AppearPosition);
            var appearCharacterView = appearCharacterInstance.GetComponent<AppearCharacterView>();

            // ViewModel 用のプレイヤーデータを作成
            PlayerAppearCharacterViewModel playerAppearCharacterViewModel = null;
            if (playerAppearCharacterReserveModel.AppearCharacterLifeDirectorType == AppearCharacterLifeDirectorType.Conversation)
            {
                playerAppearCharacterViewModel = GameManager.Instance.AppearCharacterManager.CreateWithConversationDirector(
                    appearCharacterView.transform, 
                    movePath,
                    playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase as AppearConversationCharacterDirectorModel,
                    playerAppearCharacterReserveModel);
            }
            else if (playerAppearCharacterReserveModel.AppearCharacterLifeDirectorType == AppearCharacterLifeDirectorType.Onegai)
            {
                playerAppearCharacterViewModel = GameManager.Instance.AppearCharacterManager.CreateWithOnegaiDirector(
                    appearCharacterView.transform, 
                    movePath,
                    playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase as AppearOnegaiCharacterDirectorModel,
                    playerAppearCharacterReserveModel);
            }
            else
            {
                Debug.Assert(playerAppearCharacterViewModel != null, "AppearCharacterLifeDirectorType のタイプがありません");
            }

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
