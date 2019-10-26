using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL
{
    public class ArrangementManager
    {
        /// <summary>
        /// 配置ターゲット
        /// </summary>
        private List<IArrangementTarget> arrangementTargetStore;
        public List<IArrangementTarget> ArrangementTargetStore => arrangementTargetStore;

        /// <summary>
        /// 選択されている 配置ターゲット
        /// </summary>
        private IArrangementTarget selectedArrangementTarget;

        private ArrangementPresenter arrangementPresenter;

        public ArrangementManager(ArrangementPresenter arrangementPresenter)
        {
            this.arrangementTargetStore = new List<IArrangementTarget>();
            this.arrangementPresenter = arrangementPresenter;
            this.selectedArrangementTarget = null;
        }

        /// <summary>
        /// 配置位置を追加する
        /// </summary>
        public void AddArrangement(IArrangementTarget arrangementTarget)
        {
            Debug.Assert(IsSetArrangement(arrangementTarget), "セットできない arrangementPosition が選択されています。");
            this.arrangementTargetStore.Add(arrangementTarget);
            this.arrangementPresenter.ReLoad();
        }

        /// <summary>
        /// 配置位置を消去する
        /// </summary>
        public void RemoveArranement(IArrangementTarget arrangementTarget)
        {
            // 現在登録されている位置にその位置が存在するかを確認
            if(arrangementTargetStore.Contains(arrangementTarget))
            {
                arrangementTargetStore.Remove(arrangementTarget);
            }
            this.arrangementPresenter.ReLoad();
        }

        /// <summary>
        /// 引数の配置位置がすでに存在しているかどうか？
        /// </summary>
        public bool IsSetArrangement(IArrangementTarget arrangementTarget)
        {
            foreach (var arrangementPosition in arrangementTarget.ArrangementPositions)
            {
                if (Find(arrangementPosition) != null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 選択する
        /// </summary>
        /// <param name="arrangementTarget"></param>
        public void Select(IArrangementTarget arrangementTarget)
        {
            Debug.Assert(arrangementTargetStore.Contains(arrangementTarget), "管理されていないターゲットです。");
            selectedArrangementTarget = arrangementTarget;
        }

        /// <summary>
        /// 選択されているかどうかの確認
        /// </summary>
        /// <param name="arrangementTarget"></param>
        /// <returns></returns>
        public bool CheckIsSelect(IArrangementTarget arrangementTarget)
        {
            return selectedArrangementTarget == arrangementTarget;
        }

        private ArrangementPosition Find(ArrangementPosition arrangementPosition)
        {
            var findResult = this.arrangementTargetStore.SelectMany(arrangementTarget => arrangementTarget.ArrangementPositions).ToList().Find(targetArrangementPosition =>
            {
                if (targetArrangementPosition.x != arrangementPosition.x)
                {
                    return false;
                }
                if (targetArrangementPosition.z != arrangementPosition.z)
                {
                    return false;
                }
                return true;
            });
            return findResult;
        }
    }
}
