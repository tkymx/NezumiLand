using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class FieldActionUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private Button backButton = null;

        public void Initialize()
        {
            // バックボタンを押した時
            backButton.onClick.AddListener(() =>
            {
                GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
            });

            this.Close();
        }
    }
}
