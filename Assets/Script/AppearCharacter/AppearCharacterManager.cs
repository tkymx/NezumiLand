﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private AppearCharacterChangeStateService appearCharacterChangeStateService = null;
        private AppearCharacterChangeTransformService appearCharacterChangeTransformService = null;
        private AppearCharacterSetTargetArrangementService appearCharacterSetTargetArrangementService = null;
        private AppearCharacterSetPlayingTimeService appearCharacterSetPlayingTimeService = null;

        /// <summary>
        /// パーク内の公開時のキャラクター数
        /// </summary>
        /// <param name="appearCharacterViewModel.IsParkOpenCharacter"></param>
        /// <returns></returns>
        public int ParkOpenCharacterCount => appearCharacterViewModels.Where(appearCharacterViewModel => appearCharacterViewModel.IsParkOpenCharacter).Count();

        public AppearCharacterManager(
            GameObject root, 
            IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository, 
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository,
            IPlayerAppearParkOpenCharacterDirectorRepository playerAppearParkOpenCharacterDirectorRepository)
        {
            this.root = root;

            this.appearCharacterViewModels = new List<AppearCharacterViewModel>();
            this.reservedRegisterModels = new List<AppearCharacterViewModel>();
            this.reservedRemovableModels = new List<AppearCharacterViewModel>();

            this.appearCharacterCreateService = new AppearCharacterCreateService(playerAppearConversationCharacterDirectorRepository, playerAppearParkOpenCharacterDirectorRepository, playerAppearCharacterViewRepository);
            this.appearCharacterRemoveService = new AppearCharacterRemoveService(playerAppearCharacterViewRepository);
            this.appearCharacterReceiveRewardsService = new AppearCharacterReceiveRewardsService(playerAppearConversationCharacterDirectorRepository);
            this.appearCharacterChangeStateService = new AppearCharacterChangeStateService(playerAppearCharacterViewRepository);
            this.appearCharacterChangeTransformService = new AppearCharacterChangeTransformService(playerAppearCharacterViewRepository);
            this.appearCharacterSetTargetArrangementService = new AppearCharacterSetTargetArrangementService(playerAppearCharacterViewRepository);
            this.appearCharacterSetPlayingTimeService = new AppearCharacterSetPlayingTimeService(playerAppearCharacterViewRepository);
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
                this.RemoveInternal(reservedRemovableModel);
            }
            this.reservedRemovableModels.Clear();
        }

        public PlayerAppearCharacterViewModel CreateWithConversationDirector (
            Transform view,
            MovePath movePath,
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) 
        {
            return this.appearCharacterCreateService.ExecuteWithConversatoinDirector(
                appearConversationCharacterDirectorModel.AppearCharacterModel,
                view.position,
                view.rotation.eulerAngles,
                movePath,
                appearConversationCharacterDirectorModel,
                playerAppearCharacterReserveModel
            );
        }

        public PlayerAppearCharacterViewModel CreateWithParkOpenDirector (
            Transform view,
            MovePath movePath,
            AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel) 
        {
            return this.appearCharacterCreateService.ExecuteWithParkOpenDirector(
                appearParkOpenCharacterDirectorModel.AppearCharacterModel,
                view.position,
                view.rotation.eulerAngles,
                movePath,
                appearParkOpenCharacterDirectorModel
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

        public void ToReeiveRewards (PlayerAppearConversationCharacterDirectorModel playerAppearConversationCharacterDirectorModel) {
            this.appearCharacterReceiveRewardsService.Execute(playerAppearConversationCharacterDirectorModel);
        }

        public void ChangeState (PlayerAppearCharacterViewModel playerAppearCharacterViewModel, IState state) {
            AppearCharacterState appearCharacterState = AppearCharacterState.None;
            if (state is GoMonoState) {
                appearCharacterState = AppearCharacterState.GoMono;
            }
            else if (state is GoAwayState) {
                appearCharacterState = AppearCharacterState.GoAway;
            }
            else if (state is PlayingMonoState) {
                appearCharacterState = AppearCharacterState.PlayingMono;
            }
            else if (state is RemovedState) {
                appearCharacterState = AppearCharacterState.Removed;
            }
            else if (state is EmptyState) {
                appearCharacterState = AppearCharacterState.None;
            }
            else {
                Debug.Assert(false, "状態が不定です。");
            }
            this.appearCharacterChangeStateService.Execute(playerAppearCharacterViewModel, appearCharacterState);
        }

        public void ChangeTransform (PlayerAppearCharacterViewModel playerAppearCharacterViewModel, Vector3 position, Vector3 rotation) {
            this.appearCharacterChangeTransformService.Execute(playerAppearCharacterViewModel, position, rotation);
        }

        public void SetTargetArrangement (PlayerAppearCharacterViewModel playerAppearCharacterViewModel, PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.appearCharacterSetTargetArrangementService.Execute(playerAppearCharacterViewModel, playerArrangementTargetModel);
        }

        public void SetCurrentPlayingTime (PlayerAppearCharacterViewModel playerAppearCharacterViewModel, float currentPlayingTime) {
            this.appearCharacterSetPlayingTimeService.Execute(playerAppearCharacterViewModel, currentPlayingTime);
        }

        private void RemoveInternal (AppearCharacterViewModel appearCharacterViewModel) {
            if (this.appearCharacterViewModels.IndexOf(appearCharacterViewModel) < 0) {
                return;
            }
            appearCharacterViewModel.Dispose();
            this.appearCharacterViewModels.Remove(appearCharacterViewModel);
            this.appearCharacterRemoveService.Execute(appearCharacterViewModel.PlayerAppearCharacterViewModel);
        }

        public void Remove(AppearCharacterViewModel appearCharacterViewModel) {
            this.EnqueueRemove(appearCharacterViewModel);
        }

        public void RemoveAll() {
            foreach (var reservedRegisterModel in this.appearCharacterViewModels) {
                this.Remove(reservedRegisterModel);
            }                        
        }
    }
}
