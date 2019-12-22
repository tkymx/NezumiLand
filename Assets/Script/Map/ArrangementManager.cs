using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class ArrangementManager {
        /// <summary>
        /// 配置ターゲット
        /// </summary>
        private List<IArrangementTarget> arrangementTargetStore;
        public List<IArrangementTarget> ArrangementTargetStore => arrangementTargetStore;

        /// <summary>
        /// 選択されている 配置ターゲット
        /// </summary>
        private IArrangementTarget selectedArrangementTarget;
        public IArrangementTarget SelectedArrangementTarget => selectedArrangementTarget;
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
        private Dictionary<IArrangementTarget, List<IArrangementTarget>> nearMap;

        /// <summary>
        /// 配置の予約されているものがあるか？
        /// </summary>
        /// <returns></returns>
        private bool HasReserveArrangementTarget () {
            return this.arrangementTargetStore.Find(target => target.ArrangementTargetState == ArrangementTargetState.Reserve) != null;
        }

        public ArrangementManager (GameObject root) {
            this.arrangementTargetStore = new List<IArrangementTarget> ();
            this.selectedArrangementTarget = null;
            this.arrangementAnnotater = new ArrangementAnnotater (root);
            this.nearMap = new Dictionary<IArrangementTarget, List<IArrangementTarget>>();
        }

        public void UpdateByFrame () {
            if (!GameManager.Instance.TimeManager.IsPause) {
                if (HasReserveArrangementTarget ()) {
                    AppearArrangementService.Execute ();
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
        public List<IArrangementTarget> GetNearArrangement (IArrangementTarget arrangementTarget) {
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                return new List<IArrangementTarget>();
            }
            return this.nearMap[arrangementTarget];
        }

        /// <summary>
        /// 近くの配置物を探索する
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        private List<IArrangementTarget> SearchNearArrangement (IArrangementTarget arrangementTarget) {
            var nearArrangementTargets = new List<IArrangementTarget> ();
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
        public void AddArrangement (IArrangementTarget arrangementTarget) {
            Debug.Assert (IsSetArrangement (arrangementTarget), "セットできない arrangementPosition が選択されています。");
            this.arrangementTargetStore.Add (arrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        public void CreateAndSetMono (IArrangementTarget arrangementTarget) {
            arrangementTarget.MonoViewModel = GameManager.Instance.MonoManager.CreateMono (arrangementTarget.MonoInfo, arrangementTarget.CenterPosition);
            this.AppendNearArrangement(arrangementTarget);
            GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByArrangement(arrangementTarget);
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
        public void RemoveArranement (IArrangementTarget arrangementTarget) {

            var isContain = arrangementTargetStore.Contains (arrangementTarget);
            Debug.Assert(isContain, "含まれていない配置が消されようとしました。");
            if (isContain) {
                
                arrangementTargetStore.Remove (arrangementTarget);

                // 設置済みの場合は設置情報を消す
                if (arrangementTarget.HasMonoViewModel) {
                    GameManager.Instance.MonoManager.RemoveMono (arrangementTarget.MonoViewModel);
                    GameManager.Instance.OnegaiMediaterManager.NearOnegaiMediater.MediateByRBeforeRemoval(arrangementTarget);
                    this.RemoveNearArrangement(arrangementTarget);
                }
            }
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }

        /// <summary>
        /// 近接情報を取得
        /// </summary>
        /// <param name="arrangementTarget"></param>
        private void AppendNearArrangement(IArrangementTarget arrangementTarget) {
            var nearArrangementTargets = SearchNearArrangement(arrangementTarget);
            if (!this.nearMap.ContainsKey(arrangementTarget)) {
                this.nearMap[arrangementTarget] = new List<IArrangementTarget>();
            }
            this.nearMap[arrangementTarget].AddRange(nearArrangementTargets);
            foreach (var nearArrangementTarget in nearArrangementTargets)
            {
                if (!this.nearMap.ContainsKey(nearArrangementTarget)) {
                    this.nearMap[nearArrangementTarget] = new List<IArrangementTarget>();
                }
                this.nearMap[nearArrangementTarget].Add(arrangementTarget);
            }
        }

        private void RemoveNearArrangement(IArrangementTarget arrangementTarget) {
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
        public bool IsSetArrangement (IArrangementTarget arrangementTarget) {
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
        public void Select (IArrangementTarget arrangementTarget) {
            Debug.Assert (arrangementTargetStore.Contains (arrangementTarget), "管理されていないターゲットです。");
            selectedArrangementTarget = arrangementTarget;
            GameManager.Instance.GameUIManager.ArrangementMenuUIPresenter.Show ();
        }

        /// <summary>
        /// 選択されているかどうかの確認
        /// </summary>
        /// <param name="arrangementTarget"></param>
        /// <returns></returns>
        public bool CheckIsSelect (IArrangementTarget arrangementTarget) {
            return selectedArrangementTarget == arrangementTarget;
        }

        private IArrangementTarget Find (ArrangementPosition arrangementPosition) {
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