using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiPresenter : UiWindowPresenterBase {
        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        [SerializeField]
        private Button closeButton = null;
        private IPlayerOnegaiRepository playerOnegaiRepository;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;

            this.onegaiListPresenter.Initialize (playerOnegaiRepository.GetAll ().ToList ());
            this.Close ();

            closeButton.onClick.AddListener (() => {
                this.Close ();
            });
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();

            // 表示するターゲットの判定を行う
            var targetMonoInfoId = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoInfo.Id;

            // 隣接オブジェクトの比較用データの作成
            var onegaiMediater = new OnegaiMediater (playerOnegaiRepository);

            // 近接に関するお願いのクリア可否を確認する
            var nearMonoInfoIds = GameManager.Instance.ArrangementManager
                .GetNearArrangement (GameManager.Instance.ArrangementManager.SelectedArrangementTarget)
                .Select (arrangementTarget => arrangementTarget.MonoInfo.Id)
                .ToList ();

            onegaiMediater.Mediate (new Near (nearMonoInfoIds), targetMonoInfoId);

            // 選択されているターゲットのお願いを取得
            this.onegaiListPresenter.Initialize (playerOnegaiRepository.GetByTriggerMonoInfoId (targetMonoInfoId).ToList ());
            onegaiListPresenter.ReLoad ();
        }
    }
}