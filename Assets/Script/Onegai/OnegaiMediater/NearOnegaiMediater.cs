using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class NearOnegaiMediater {

        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;
        private readonly OnegaiMediater onegaiMediater = null;

        private Dictionary<uint, List<OnegaiModel>> monoInfoIdToOngeais;

        public NearOnegaiMediater (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.onegaiMediater = new OnegaiMediater(playerOnegaiRepository);
            this.monoInfoIdToOngeais = new Dictionary<uint, List<OnegaiModel>>();
        }

        /// <summary>
        /// 設置時の判定
        /// 設置したオブジェクトと、その隣接オブジェクトに関して判定する
        /// </summary>
        /// <param name="arrangementTarget"></param>
        public void MediateByArrangement(IPlayerArrangementTarget arrangementTarget) 
        {
            var nearArrangementTargets = GameManager.Instance.ArrangementManager.GetNearArrangement (arrangementTarget);
            foreach (var nearArrangementTarget in nearArrangementTargets) {
                var nearNearArrangementTarget = GameManager.Instance.ArrangementManager.GetNearArrangement(nearArrangementTarget);
                this.MediateInternal(nearArrangementTarget, nearNearArrangementTarget, false);
            }
            this.MediateInternal(arrangementTarget,nearArrangementTargets, false);
        }

        /// <summary>
        /// 破棄時の判定
        /// 隣接オブジェクトと、自分のお願いを判定
        /// </summary>
        /// <param name="arrangementTarget"></param>
        public void MediateByRBeforeRemoval(IPlayerArrangementTarget arrangementTarget) 
        {
            var nearArrangementTargets = GameManager.Instance.ArrangementManager.GetNearArrangement (arrangementTarget);
            foreach (var nearArrangementTarget in nearArrangementTargets) {
                var nearNearArrangementTargets = GameManager.Instance.ArrangementManager.GetNearArrangement(nearArrangementTarget);
                nearNearArrangementTargets.Remove(arrangementTarget);
                this.MediateInternal(nearArrangementTarget, nearNearArrangementTargets, true);
            }
            this.MediateInternal(arrangementTarget,new List<IPlayerArrangementTarget>(), true);
        }

        private void MediateInternal(IPlayerArrangementTarget arrangementTarget, List<IPlayerArrangementTarget> nearArrangementTargets,  bool isReset) {
            var targetMonoInfoId = arrangementTarget.MonoInfo.Id;                
            if (!monoInfoIdToOngeais.ContainsKey(targetMonoInfoId)) {
                return;
            }

            // 近隣のMonoIDを取得
            var nearMonoInfoIds = nearArrangementTargets
                    .Select (nearArrangementTarget => nearArrangementTarget.MonoInfo.Id)
                    .ToList ();

            // 紐付いたお願いのIDを取得
            var onegaiModelIds = this.monoInfoIdToOngeais[targetMonoInfoId]
                .Select(model => model.Id)
                .ToList();
            
            // お願いのIDからプレイヤー情報を取得s
            var playerOnegaiModels = this.playerOnegaiRepository.GetByIds(onegaiModelIds);

            // 一旦クリア済みのものもリセットしてクリア状況を判断する
            if (isReset) {
                onegaiMediater.ResetAndMediate (
                    new Near (targetMonoInfoId, nearMonoInfoIds), 
                    playerOnegaiModels);
            } else {
                onegaiMediater.Mediate (
                    new Near (targetMonoInfoId, nearMonoInfoIds), 
                    playerOnegaiModels);
            }
        }

        public void ChacheOnegai(OnegaiModel onegaiModel) 
        {
            var nearArgs = new NearArgs(onegaiModel.OnegaiConditionArg);
            if (!this.monoInfoIdToOngeais.ContainsKey(nearArgs.TargetMonoInfoId)) {
                this.monoInfoIdToOngeais[nearArgs.TargetMonoInfoId] = new List<OnegaiModel>();
            }
            if (this.monoInfoIdToOngeais[nearArgs.TargetMonoInfoId].Contains(onegaiModel)) {
                return;
            }
            this.monoInfoIdToOngeais[nearArgs.TargetMonoInfoId].Add(onegaiModel);
        }

        public void UnChacheOnegai(OnegaiModel onegaiModel) 
        {
            var nearArgs = new NearArgs(onegaiModel.OnegaiConditionArg);
            this.monoInfoIdToOngeais[nearArgs.TargetMonoInfoId].RemoveAll(model => model.Id == onegaiModel.Id);
        }        
    }
}