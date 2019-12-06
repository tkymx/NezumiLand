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

        // Start is called before the first frame update
        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            deleteButton.onClick.AddListener (() => {
                var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
                if (GameManager.Instance.Wallet.IsPay (removeFee)) {
                    GameManager.Instance.Wallet.Pay (removeFee);
                    GameManager.Instance.EffectManager.PlayConsumeEffect (removeFee, GameManager.Instance.ArrangementManager.SelectedArrangementTarget.CenterPosition);
                    GameManager.Instance.ArrangementManager.RemoveSelectArrangement ();
                    this.DoFinishProcess ();
                }
            });

            levelUpButton.onClick.AddListener (() => {
                var levelUpFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
                if (GameManager.Instance.Wallet.IsPay (levelUpFee)) {
                    GameManager.Instance.Wallet.Pay (levelUpFee);
                    GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.LevelUp ();
                    GameManager.Instance.EffectManager.PlayConsumeEffect (levelUpFee, GameManager.Instance.ArrangementManager.SelectedArrangementTarget.CenterPosition);
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
            UpdateDetail ();
            UpdateRemoveButtonEnable ();
            UpdateRemoveFee ();
            UpdateLevelUpButtonEnable ();
            UpdateLevelUpFee ();
        }

        public void Show () {
            arrangementUI.SetActive (true);
            Update ();
        }

        public void Close () {
            arrangementUI.SetActive (false);
        }

        private void UpdateDetail () {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget) {
                detail.text = "未選択状態";
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.HasMonoViewModel) {
                detail.text = "建築中";
                return;
            }

            detail.text = "Level:" + GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.CurrentLevel;
            return;
        }

        private void UpdateRemoveButtonEnable () {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget) {
                deleteButton.interactable = false;
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.HasMonoViewModel) {
                deleteButton.interactable = false;
                return;
            }

            var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
            if (!GameManager.Instance.Wallet.IsPay (removeFee)) {
                deleteButton.interactable = false;
                return;
            }

            deleteButton.interactable = true;
        }

        private void UpdateRemoveFee () {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget) {
                deleteFee.text = "";
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.HasMonoViewModel) {
                deleteFee.text = "";
                return;
            }

            var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
            deleteFee.text = removeFee.ToString ();
        }

        private void UpdateLevelUpButtonEnable () {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget) {
                levelUpButton.interactable = false;
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.HasMonoViewModel) {
                levelUpButton.interactable = false;
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.ExistNextLevelUp ()) {
                levelUpButton.interactable = false;
                return;
            }

            var levelUpFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
            if (!GameManager.Instance.Wallet.IsPay (levelUpFee)) {
                levelUpButton.interactable = false;
                return;
            }

            levelUpButton.interactable = true;
        }

        private void UpdateLevelUpFee () {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget) {
                levelUpFeeText.text = "";
                return;
            }
            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.HasMonoViewModel) {
                levelUpFeeText.text = "";
                return;
            }

            if (!GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.ExistNextLevelUp ()) {
                levelUpFeeText.text = "最大レベルです";
                return;
            }

            var levelUpFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.GetCurrentLevelUpFee ();
            levelUpFeeText.text = levelUpFee.ToString ();
        }
    }
}