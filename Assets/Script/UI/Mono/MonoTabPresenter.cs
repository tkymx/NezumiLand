using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class MonoTabPresenter : UiWindowPresenterBase {
        
        private IPlayerMonoInfoRepository playerMonoInfoRepository = null;

        [SerializeField]
        private List<Button> tabButtons = null;

        [SerializeField]
        private List<MonoType> displayMonoType = null;

        [SerializeField]
        private MonoListPresenter monoListPresetner = null;

        public void Initialize (IPlayerMonoInfoRepository playerMonoInfoRepository) {
            this.playerMonoInfoRepository = playerMonoInfoRepository;

            Debug.Assert (this.tabButtons.Count > 0, "タブの項目がありません");
            Debug.Assert (this.tabButtons.Count == this.displayMonoType.Count, "ボタンとタイプの個数が異なります。");

            for (int buttonIndex = 0; buttonIndex < this.tabButtons.Count; buttonIndex++) {
                var button = this.tabButtons[buttonIndex];
                var monoType = this.displayMonoType[buttonIndex];

                button.onClick.AddListener (() => {
                    this.SelectTab (button, monoType);
                });
            }
            this.Close ();
        }

        private void SelectTab (Button selectButton, MonoType selectType) {
            // すべてのボタンをアクティブにする
            foreach (var button in tabButtons) {
                button.interactable = true;
            }

            // 押したボタンを非アクティブにする
            selectButton.interactable = false;

            // 表示可能なモノを取得
            var displayableMonoInfos = playerMonoInfoRepository.GetDisplayableByType(selectType).ToList();
            this.monoListPresetner.SetElement (displayableMonoInfos);
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.SelectTab (tabButtons[0], displayMonoType[0]);
            monoListPresetner.ReLoad ();
        }
    }
}