using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class MonoTabPresenter : UiWindowPresenterBase {
        [SerializeField]
        private List<Button> tabButtons = null;

        [SerializeField]
        private List<MonoType> displayMonoType = null;

        [SerializeField]
        private MonoListPresenter monoListPresetner = null;

        public void Initialize () {
            Debug.Assert (this.tabButtons.Count > 0, "タブの項目がありません");
            Debug.Assert (this.tabButtons.Count == this.displayMonoType.Count, "ボタンとタイプの個数が異なります。");

            for (int buttonIndex = 0; buttonIndex < this.tabButtons.Count; buttonIndex++) {
                var button = this.tabButtons[buttonIndex];
                var monoType = this.displayMonoType[buttonIndex];

                button.onClick.AddListener (() => {
                    this.SelectTab (button, monoType);
                });
            }

            this.SelectTab (tabButtons[0], displayMonoType[0]);

            this.Close ();
        }

        private void SelectTab (Button selectButton, MonoType selectType) {
            // すべてのボタンをアクティブにする
            foreach (var button in tabButtons) {
                button.interactable = true;
            }

            // 押したボタンを非アクティブにする
            selectButton.interactable = false;

            var monoInfoRepository = new MonoInfoRepository (ContextMap.DefaultMap);
            this.monoListPresetner.SetElement (monoInfoRepository.GetByType (selectType).ToList ());
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            monoListPresetner.ReLoad ();
        }
    }
}