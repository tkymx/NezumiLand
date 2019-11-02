using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    /// <summary>
    /// ものを選択したときのメニュー
    /// </summary>
	public class ArrangementMenuUIPresenter : MonoBehaviour
	{
        [SerializeField]
        private GameObject arrangementUI = null;

        [SerializeField]
        private Button deleteButton = null;

        [SerializeField]
        private Text deleteFee = null;

        [SerializeField]
        private Button closeButton = null;

        // Start is called before the first frame update
        public void Initialize()
		{
            deleteButton.onClick.AddListener(() =>
            {
                var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
                if (GameManager.Instance.Wallet.IsPay(removeFee))
                {
                    GameManager.Instance.Wallet.Pay(removeFee);
                    GameManager.Instance.EffectManager.PlayRemoveMonoEffect(removeFee, GameManager.Instance.ArrangementManager.SelectedArrangementTarget.CenterPosition);
                    GameManager.Instance.ArrangementManager.RemoveSelectArrangement();
                    this.Close();
                }
            });

            closeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ArrangementManager.RemoveSelection();
                this.Close();
            });

            // 初めは閉じておく
            this.Close();
        }

        private void Update()
        {
            UpdateRemoveButtonEnable();
            UpdateRemoveFee();
        }

        public void Show()
        {
            arrangementUI.SetActive(true);
            UpdateRemoveButtonEnable();
        }

        public void Close()
        {
            arrangementUI.SetActive(false);
        }

        private void UpdateRemoveButtonEnable()
        {
            if (!GameManager.Instance.ArrangementManager.IsRemoveSelectArrangement())
            {
                deleteButton.interactable = false;
                return;
            }

            var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
            if (!GameManager.Instance.Wallet.IsPay(removeFee))
            {
                deleteButton.interactable = false;
                return;
            }

            deleteButton.interactable = true;
        }

        private void UpdateRemoveFee()
        {
            if (!GameManager.Instance.ArrangementManager.HasSelectedArrangementTarget)
            {
                return;
            }
            if (!GameManager.Instance.ArrangementManager.IsRemoveSelectArrangement())
            {
                return;
            }
            var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveFee;
            deleteFee.text = removeFee.ToString();
        }
    }
}