using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class ArrangementManager {

        /// <summary>
        /// 配置ターゲット
        /// </summary>
        private List<IPlayerArrangementTarget> arrangementTargetStore;
        public List<IPlayerArrangementTarget> ArrangementTargetStore => arrangementTargetStore;

        /// <summary>
        /// 選択されている 配置ターゲット
        /// </summary>
        private IPlayerArrangementTarget selectedArrangementTarget;
        public IPlayerArrangementTarget SelectedArrangementTarget => selectedArrangementTarget;
        public bool HasSelectedArrangementTarget => selectedArrangementTarget != null;

        /// <summary>
        /// 選択状況
        /// </summary>
        private ArrangementAnnotater arrangementAnnotater;
        public ArrangementAnnotater ArrangementAnnotater => arrangementAnnotater;

        public bool IsEnable {
            get {
                return GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo;
            }
        }

        /// <summary>
        /// 隣接状況を保管している
        /// </summary>
        private Dictionary<IPlayerArrangementTarget, List<IPlayerArrangementTarget>> nearMap;

        /// <summary>
        /// 配置の予約されているものがあるか？
        /// </summary>
        /// <returns></returns>
        private bool HasReserveArrangementTarget () {
            return this.arrangementTargetStore.Find(target => target.ArrangementTargetState == ArrangementTargetState.Reserve) != null;
        }

        /// <summary>
        /// モノViewのセットのサービス
        /// </summary>
        private SetMonoViewModelToArrangementService setMonoViewModelToArrangementService = null;

        /// <summary>
        /// 配置物が消去できない
        /// </summary>
        private ArrangementTargetRemoveService arrangementTargetRemoveService = null;

        /// <summary>
        /// 予約状態から可視化する際に使用する
        /// </summary>
        private AppearArrangementService appearArrangementService = null;

        private IPlayerOnegaiRepository playerOnegaiRepository = null;

        private OnegaiMediater onegaiMediater = null;

        public ArrangementManager (GameObject root, IPlayerOnegaiRepository playerOnegaiRepository, IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.arrangementTargetStore = new List<IPlayerArrangementTarget> ();
            this.selectedArrangementTarget = null;
            this.arrangementAnnotater = new ArrangementAnnotater (root);
            this.nearMap = new Dictionary<IPlayerArrangementTarget, List<IPlayerArrangementTarget>>();
            
            this.setMonoViewModelToArrangementService = new SetMonoViewModelToArrangementService(playerArrangementTargetRepository);
            this.arrangementTargetRemoveService = new ArrangementTargetRemoveService(playerArrangementTargetRepository);
            this.appearArrangementService = new AppearArrangementService(playerArrangementTargetRepository);

            this.onegaiMediater = new OnegaiMediater(playerOnegaiRepository);
            this.playerOnegaiRepository = playerOnegaiRepository;
        }

        public void UpdateByFrame () {
            if (!GameManager.Instance.TimeManager.IsPause) {
                if (HasReserveArrangementTarget ()) {
                    this.appearArrangementService.Execute ();
                }
            }
        }

        /// <summary>
        /// id 指定して今どのくらい出現しているのかを調べる
        /// ※都度計算しており低速なので後でテーブル化はしたい。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>出現数</returns>
        public int GetAppearMonoCountById (uint id, bool withReserve = true) {
            var result = this.arrangementTargetStore;
            if (!withReserve) {
                result = this.arrangementTargetStore
                    .Where (target => target.ArrangementTargetState == ArrangementTargetState.Appear)
                    .ToList();
            }
            return result
                .Where (target => target.HasMonoInfo)
                .Where (target => target.MonoInfo.Id == id)
                .Count ();
        }

        /// <summary>
        /// 近くの配置物を取得する
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        public List<IPlayerArrangementTarget> GetNearArrangement (IPlayerArrangementTarget arrangementTarget) {
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                return new List<IPlayerArrangementTarget>();
            }
            return this.nearMap[arrangementTarget];
        }

        /// <summary>
        /// 近くの配置物を探索する
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        private List<IPlayerArrangementTarget> SearchNearArrangement (IPlayerArrangementTarget arrangementTarget) {
            var nearArrangementTargets = new List<IPlayerArrangementTarget> ();
            foreach (var arrangementPosition in arrangementTarget.GetEdgePositions ()) {
                var findArrangementTarget = this.Find (arrangementPosition);
                if (findArrangementTarget == null) {
                    continue;
                }
                if (nearArrangementTargets.IndexOf (findArrangementTarget) >= 0) {
                    continue;
                }
                if (!findArrangementTarget.HasMonoInfo) {
                    continue;
                }
                nearArrangementTargets.Add (findArrangementTarget);
            }
            return nearArrangementTargets;
        }

        /// <summary>
        /// 配置位置を追加する
        /// </summary>
        public void AddArrangement (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert (IsSetArrangement (arrangementTarget), "セットできない arrangementPosition が選択されています。");
            this.arrangementTargetStore.Add (arrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        public void CreateAndSetMono (PlayerArrangementTargetModel playerArrangementTargetModel) {
            var monoViewModel = GameManager.Instance.MonoManager.CreateMono (playerArrangementTargetModel.MonoInfo, playerArrangementTargetModel.CenterPosition);
            var foundArrangementTarget = this.arrangementTargetStore.Find(arrangementTarget => arrangementTarget.PlayerArrangementTargetModel.Equals(playerArrangementTargetModel));
            Debug.Assert(foundArrangementTarget != null, "配置ターゲットがありません");

            // 生成した viewModel をセット
            this.setMonoViewModelToArrangementService.Execute(foundArrangementTarget, monoViewModel);

            // お願いの確認

            this.AppendNearArrangement(foundArrangementTarget);
            GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByArrangement(foundArrangementTarget);

            // 通常の判断
            var arrangemntMonoId = playerArrangementTargetModel.MonoInfo.Id;
            this.onegaiMediater.Mediate (
                new NL.OnegaiConditions.ArrangementCount(arrangemntMonoId, (uint)GetAppearMonoCountById(arrangemntMonoId, false)),
                playerOnegaiRepository.GetAll().ToList()
            ); 
        }

        /// <summary>
        /// 選択を外す
        /// </summary>
        public void RemoveSelection () {
            this.selectedArrangementTarget = null;
            GameManager.Instance.ArrangementPresenter.ReLoad ();
            GameManager.Instance.GameUIManager.ArrangementMenuUIPresenter.Close ();
        }

        /// <summary>
        /// 選択されている配置ターゲットを消す
        /// </summary>
        public void RemoveSelectArrangement () {
            this.RemoveArranement (this.selectedArrangementTarget);
            this.selectedArrangementTarget = null;
            GameManager.Instance.ArrangementPresenter.ReLoad ();
            GameManager.Instance.GameUIManager.ArrangementMenuUIPresenter.Close ();
        }

        /// <summary>
        /// 配置位置を消去する
        /// </summary>
        public void RemoveArranement (IPlayerArrangementTarget arrangementTarget) {

            var isContain = arrangementTargetStore.Contains (arrangementTarget);
            Debug.Assert(isContain, "含まれていない配置が消されようとしました。");
            if (isContain) {

                // 設置済みの場合は設置情報を消す
                if (arrangementTarget.HasMonoViewModel) {
                    GameManager.Instance.MonoManager.RemoveMono (arrangementTarget.MonoViewModel);
                    GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByRBeforeRemoval(arrangementTarget);

                    // 近隣配置のお願い（自分の近隣を見るために消える前に必要）
                    this.RemoveNearArrangement(arrangementTarget);
                }

                this.arrangementTargetStore.Remove (arrangementTarget);
                this.arrangementTargetRemoveService.Execute(arrangementTarget);

                // 設置済みの場合は設置情報を消す
                if (arrangementTarget.HasMonoViewModel) {

                    // 減ったかどうかの判断（減った状態を渡すため、減ったあとに事項する必要あり）
                    var arrangemntMonoId = arrangementTarget.MonoInfo.Id;
                    this.onegaiMediater.ClearResetAndMediate (
                        new NL.OnegaiConditions.ArrangementCount(arrangemntMonoId, (uint)GetAppearMonoCountById(arrangemntMonoId, false)),
                        playerOnegaiRepository.GetAll().ToList()
                    );
                }
            }
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        /// <summary>
        /// 近接情報を取得
        /// </summary>
        /// <param name="arrangementTarget"></param>
        private void AppendNearArrangement(IPlayerArrangementTarget arrangementTarget) {
            var nearArrangementTargets = SearchNearArrangement(arrangementTarget);
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                this.nearMap[arrangementTarget] = new List<IPlayerArrangementTarget>();
            }
            this.nearMap[arrangementTarget].AddRange(nearArrangementTargets);
            foreach (var nearArrangementTarget in nearArrangementTargets)
            {
                if (!this.nearMap.ContainsKey(nearArrangementTarget)) {
                    this.nearMap[nearArrangementTarget] = new List<IPlayerArrangementTarget>();
                }
                this.nearMap[nearArrangementTarget].Add(arrangementTarget);
            }
        }

        private void RemoveNearArrangement(IPlayerArrangementTarget arrangementTarget) {
            if (this.nearMap.ContainsKey(arrangementTarget)) {
                foreach (var nearArrangementTarget in this.nearMap[arrangementTarget])
                {
                    this.nearMap[nearArrangementTarget].Remove(arrangementTarget);
                }
                this.nearMap.Remove(arrangementTarget);
            }
        }

        /// <summary>
        /// 引数の配置位置がすでに存在しているかどうか？
        /// </summary>
        public bool IsSetArrangement (IPlayerArrangementTarget arrangementTarget) {
            return IsSetArrangement (arrangementTarget.ArrangementPositions);
        }
        public bool IsSetArrangement (List<ArrangementPosition> arrangementPositions) {
            foreach (var arrangementPosition in arrangementPositions) {
                if (Find (arrangementPosition) != null) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 選択する
        /// </summary>
        /// <param name="arrangementTarget"></param>
        public void Select (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert (arrangementTargetStore.Contains (arrangementTarget), "管理されていないターゲットです。");
            selectedArrangementTarget = arrangementTarget;
            GameManager.Instance.GameUIManager.ArrangementMenuUIPresenter.Show ();
        }

        /// <summary>
        /// 選択されているかどうかの確認
        /// </summary>
        /// <param name="arrangementTarget"></param>
        /// <returns></returns>
        public bool CheckIsSelect (IPlayerArrangementTarget arrangementTarget) {
            return selectedArrangementTarget == arrangementTarget;
        }

        private IPlayerArrangementTarget Find (ArrangementPosition arrangementPosition) {
            foreach (var arrangemetTarget in this.arrangementTargetStore) {
                var findIndex = arrangemetTarget.ArrangementPositions.FindIndex (targetArrangementPosition => {
                    if (targetArrangementPosition.x != arrangementPosition.x) {
                        return false;
                    }
                    if (targetArrangementPosition.z != arrangementPosition.z) {
                        return false;
                    }
                    return true;
                });

                // 見つかったら返却
                if (findIndex >= 0) {
                    return arrangemetTarget;
                }
            }
            return null;
        }
    }
}