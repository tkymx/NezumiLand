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

        public ArrangementManager (GameObject root) {
            this.arrangementTargetStore = new List<IArrangementTarget> ();
            this.selectedArrangementTarget = null;
            this.arrangementAnnotater = new ArrangementAnnotater (root);
        }

        /// <summary>
        /// id 指定して今どのくらい出現しているのかを調べる
        /// ※都度計算しており低速なので後でテーブル化はしたい。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>出現数</returns>
        public int GetAppearMonoCountById (uint id) {
            return this.arrangementTargetStore
                .Where (target => target.HasMonoInfo)
                .Where (target => target.MonoInfo.Id == id)
                .Count ();
        }

        /// <summary>
        /// 近くの配置物を取得
        /// </summary>
        /// <param name="arrangementTarget">モデル</param>
        /// <returns></returns>
        public List<IArrangementTarget> GetNearArrangement (IArrangementTarget arrangementTarget) {
            var nearArrangementTargets = new List<IArrangementTarget> ();
            foreach (var arrangementPosition in arrangementTarget.GetEdgePositions ()) {
                var findArrangementTarget = this.Find (arrangementPosition);
                if (findArrangementTarget == null) {
                    continue;
                }
                if (nearArrangementTargets.IndexOf (findArrangementTarget) >= 0) {
                    continue;
                }
                if (!findArrangementTarget.HasMonoViewModel) {
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
            // 現在登録されている位置にその位置が存在するかを確認
            if (arrangementTargetStore.Contains (arrangementTarget)) {
                arrangementTargetStore.Remove (arrangementTarget);
                GameManager.Instance.MonoManager.RemoveMono (arrangementTarget.MonoViewModel);
            }
            GameManager.Instance.ArrangementPresenter.ReLoad ();
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