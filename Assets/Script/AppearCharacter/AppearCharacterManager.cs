using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public class AppearCharacterManager
    {
        private GameObject root = null;
        public GameObject Root => root;

        private List<AppearCharacterViewModel> appearCharacterViewModels = null;
        private List<AppearCharacterViewModel> reservedRegisterModels = null;
        private List<AppearCharacterViewModel> reservedRemovableModels = null;

        private AppearCharacterCreateService appearCharacterCreateService = null;
        private AppearCharacterRemoveService appearCharacterRemoveService = null;
        private AppearCharacterReceiveRewardsService appearCharacterReceiveRewardsService = null;

        public AppearCharacterManager(GameObject root, IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.root = root;

            this.appearCharacterViewModels = new List<AppearCharacterViewModel>();
            this.reservedRegisterModels = new List<AppearCharacterViewModel>();
            this.reservedRemovableModels = new List<AppearCharacterViewModel>();

            this.appearCharacterCreateService = new AppearCharacterCreateService(playerAppearCharacterViewRepository);
            this.appearCharacterRemoveService = new AppearCharacterRemoveService(playerAppearCharacterViewRepository);
            this.appearCharacterReceiveRewardsService = new AppearCharacterReceiveRewardsService(playerAppearCharacterViewRepository);
        }

        public void UpdateByFrame()
        {
            // 追加
            foreach (var reservedRegisterModel in this.reservedRegisterModels)
            {
                this.Register(reservedRegisterModel);                
            }
            this.reservedRegisterModels.Clear();

            // 更新      
            foreach (var appearCharacterViewModel in appearCharacterViewModels)
            {
                appearCharacterViewModel.UpdateByFrame();                
            }

            // 消去
            foreach (var reservedRemovableModel in this.reservedRemovableModels)
            {
                this.Remove(reservedRemovableModel);
            }
            this.reservedRemovableModels.Clear();
        }

        public PlayerAppearCharacterViewModel Create (Transform view, PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            return this.appearCharacterCreateService.Execute(
                view.position,
                view.rotation.eulerAngles,
                playerAppearCharacterReserveModel
            );
        }

        public void EnqueueRegister (AppearCharacterViewModel appearCharacterViewModel) {
            this.reservedRegisterModels.Add(appearCharacterViewModel);
        }

        private void Register(AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModels.Add(appearCharacterViewModel);
        }

        private void EnqueueRemove (AppearCharacterViewModel appearCharacterViewModel) {
            this.reservedRemovableModels.Add(appearCharacterViewModel);
        }

        public void ToReeiveRewards (PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            playerAppearCharacterViewModel.ToReceiveRewards();
            this.appearCharacterReceiveRewardsService.Execute(playerAppearCharacterViewModel);
        }

        private void Remove (AppearCharacterViewModel appearCharacterViewModel) {
            if (this.appearCharacterViewModels.IndexOf(appearCharacterViewModel) < 0) {
                return;
            }
            appearCharacterViewModel.Dispose();
            this.appearCharacterViewModels.Remove(appearCharacterViewModel);
            this.appearCharacterRemoveService.Execute(appearCharacterViewModel.PlayerAppearCharacterViewModel);
        }

        public void RemoveAll() {
            foreach (var reservedRegisterModel in this.appearCharacterViewModels) {
                this.EnqueueRemove(reservedRegisterModel);
            }                        
        }
    }
}
