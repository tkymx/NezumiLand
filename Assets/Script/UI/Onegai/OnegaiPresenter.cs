using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace NL
{
    public class OnegaiPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        [SerializeField]
        private Button closeButton = null;

        public void Initialize()
        {
            // todo 本当は開くタイミングでmono info id で絞った結果を出したい
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap);
            var playerOnegaiRepository = new PlayerOnegaiRepository(onegaiRepository);
            this.onegaiListPresenter.Initialize(playerOnegaiRepository.GetAll().ToList());
            this.Close();

            closeButton.onClick.AddListener(() =>
            {
                this.Close();
            });
        }

        public override void onPrepareShow()
        {
            base.onPrepareShow();
            onegaiListPresenter.ReLoad();
        }
    }
}

