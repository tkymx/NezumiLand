using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ArrangementModeUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private Button backButton = null;

        public void Initialize()
        {
            // バックボタンを押した時
            backButton.onClick.AddListener(() =>
            {
                // モードを戻す
                GameManager.Instance.GameModeManager.Back();
            });

            this.Close();
        }
    }
}
