using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    /// <summary>
    /// ものを選択したときのメニュー
    /// </summary>
    public class ArrangementMenuUIPresenter : MonoBehaviour {
        [SerializeField]
        private GameObject arrangementUI = null;

        [SerializeField]
        private Text monoName = null;

        [SerializeField]
        private Text detail = null;

        [SerializeField]
        private Button deleteButton = null;

        [SerializeField]
        private Text deleteFee = null;

        [SerializeField]
        private Button levelUpButton = null;

        [SerializeField]
        private Text levelUpFeeText = null;

        [SerializeField]
        private Button closeButton = null;

        private IPlayerArrangementTarget selectedArrangementTarget = null;

        // Start is called before the first frame update
        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            deleteButton.onClick.AddListener (() => {
                var removeFee = this.selectedArrangementTarget.MonoViewModel.RemoveFee;
                var item = this.selectedArrangementTarget.MonoInfo.ArrangementItemAmount;
                if (GameManager.Instance.Wallet.IsConsume (removeFee)) {
                    GameManager.Instance.Wallet.Consume (removeFee);
                    GameManager.Instance.ArrangementItemStore.Push (item);
                    GameManager.Instance.EffectManager.PlayConsumeEffect (removeFee, this.selectedArrangementTarget.CenterPosition);
                    GameManager.Instance.EffectManager.PlayEarnItemEffect (item, this.selectedArrangementTarget.CenterPosition + new Vector3(0,2.0f,0));
                    GameManager.Instance.ArrangementManager.RemoveSelectArrangement ();
                    this.DoFinishProcess ();
                }
            });

            levelUpButton.onClick.AddListener (() => {
                var levelUpFee = this.selectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
                if (GameManager.Instance.Wallet.IsConsume (levelUpFee)) {
                    GameManager.Instance.Wallet.Consume (levelUpFee);
                    this.selectedArrangementTarget.MonoViewModel.LevelUp ();
                    GameManager.Instance.EffectManager.PlayConsumeEffect (levelUpFee, this.selectedArrangementTarget.CenterPosition);
                }
            });

            closeButton.onClick.AddListener (() => {
                GameManager.Instance.ArrangementManager.RemoveSelection ();
                this.DoFinishProcess ();
            });

            // 初めは閉じておく
            this.Close ();
        }

        private void DoFinishProcess () {
            GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateSelectMode ());
            this.Close ();
        }

        private void Update () {
            UpdateName ();
            UpdateDetail ();
            UpdateRemoveButtonEnable ();
            UpdateRemoveFee ();
            UpdateLevelUpButtonEnable ();
            UpdateLevelUpFee ();
        }

        public void Show (IPlayerArrangementTarget selectedArrangementTarget) {
            this.selectedArrangementTarget = selectedArrangementTarget;
            arrangementUI.SetActive (true);
            Update ();
        }

        public void Close () {
            this.selectedArrangementTarget = null;
            arrangementUI.SetActive (false);
        }

        private void UpdateName () {
            if (this.selectedArrangementTarget == null) {
                return;
            }

            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                return;
            }

            monoName.text = this.selectedArrangementTarget.MonoInfo.Name;
        }

        private void UpdateDetail () {
            if (this.selectedArrangementTarget == null) {
                detail.text = "未選択状態";
                return;
            }

            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                detail.text = "建築中";
                return;
            }

            detail.text = "Level:" + this.selectedArrangementTarget.MonoViewModel.CurrentLevel;
            return;
        }

        private void UpdateRemoveButtonEnable () {
            if (this.selectedArrangementTarget == null) {
                deleteButton.interactable = false;
                return;
            }

            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                deleteButton.interactable = false;
                return;
            }

            var removeFee = this.selectedArrangementTarget.MonoViewModel.RemoveFee;
            if (!GameManager.Instance.Wallet.IsConsume (removeFee)) {
                deleteButton.interactable = false;
                return;
            }

            deleteButton.interactable = true;
        }

        private void UpdateRemoveFee () {
            if (this.selectedArrangementTarget == null) {
                deleteFee.text = "";
                return;
            }

            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                deleteFee.text = "";
                return;
            }

            var removeFee = this.selectedArrangementTarget.MonoViewModel.RemoveFee;
            deleteFee.text = removeFee.ToString ();
        }

        private void UpdateLevelUpButtonEnable () {
            if (this.selectedArrangementTarget == null) {
                levelUpButton.interactable = false;
                return;
            }

            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                levelUpButton.interactable = false;
                return;
            }

            if (!this.selectedArrangementTarget.MonoViewModel.ExistNextLevelUp ()) {
                levelUpButton.interactable = false;
                return;
            }

            var levelUpFee = this.selectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
            if (!GameManager.Instance.Wallet.IsConsume (levelUpFee)) {
                levelUpButton.interactable = false;
                return;
            }

            levelUpButton.interactable = true;
        }

        private void UpdateLevelUpFee () {
            if (this.selectedArrangementTarget == null) {
                levelUpFeeText.text = "";
                return;
            }
            if (!this.selectedArrangementTarget.HasMonoViewModel) {
                levelUpFeeText.text = "";
                return;
            }

            if (!this.selectedArrangementTarget.MonoViewModel.ExistNextLevelUp ()) {
                levelUpFeeText.text = "最大レベルです";
                return;
            }

            var levelUpFee = this.selectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
            levelUpFeeText.text = levelUpFee.ToString ();
        }
    }
}