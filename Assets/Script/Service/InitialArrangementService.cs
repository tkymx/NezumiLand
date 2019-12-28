using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InitialArrangementService
    {
        private IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public InitialArrangementService(IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        public void Execute() {
            
            foreach (var playerArrangementTargetModel in playerArrangementTargetRepository.GetAll())
            {
                // 配置
                var playerArrangementTarget = new PlayerArrangementTarget(playerArrangementTargetModel);

                // 予約の場合は消す
                if (playerArrangementTarget.PlayerArrangementTargetModel.State == ArrangementTargetState.Reserve) {
                    this.playerArrangementTargetRepository.Remove(playerArrangementTarget.PlayerArrangementTargetModel);
                    continue;
                }

                // 配置
                GameManager.Instance.ArrangementManager.AddArrangement(playerArrangementTarget);

                // ものの配置
                var playreMonoViewModel = playerArrangementTarget.PlayerArrangementTargetModel.PlayerMonoViewModel;
                if (playreMonoViewModel != null) {
                    var monoViewModel = GameManager.Instance.MonoManager.CreateMono(playreMonoViewModel, playerArrangementTarget.CenterPosition);
                    playerArrangementTarget.RegisterMade(monoViewModel);
                }
            }
        }
    }   
}
