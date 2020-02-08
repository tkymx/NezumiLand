using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class FieldActionUIPresenter : UiWindowPresenterBase {
        [SerializeField]
        private Button backButton = null;

        [SerializeField]
        private Button startButton = null;

        [SerializeField]
        private GameObject yetArrangementMessage = null;

        public void Initialize () {
            // バックボタンを押した時
            backButton.onClick.AddListener (() => {
                GameManager.Instance.ArrangementManager.UnReserveArrangementAll();
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateMenuSelectMode ());
            });
            // スタートボタンを押した時
            startButton.onClick.AddListener (() => {
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateSelectMode ());
            });

            this.Close ();
        }

        public void UpdateByFrame () {
            if (!IsShow()) {
                return;
            }
            
            yetArrangementMessage.SetActive(!GameManager.Instance.ArrangementManager.HasReserveArrangementTarget ());
            startButton.interactable = GameManager.Instance.ArrangementManager.HasReserveArrangementTarget ();
        }
    }
}