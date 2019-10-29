using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
	public class ArrangementUIPresenter : MonoBehaviour
	{
        [SerializeField]
        private GameObject arrangementUI = null;

        [SerializeField]
        private Button deleteButton = null;

        [SerializeField]
        private Button closeButton = null;

        // Start is called before the first frame update
        void Start()
		{
            deleteButton.onClick.AddListener(() =>
            {
                var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveCurrency;
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
        }

        private void Update()
        {
            UpdateRemoveButtonEnable();
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
                deleteButton.gameObject.SetActive(false);
                return;
            }

            var removeFee = GameManager.Instance.ArrangementManager.SelectedArrangementTarget.MonoViewModel.RemoveCurrency;
            var isEnableRemoveMono = GameManager.Instance.Wallet.IsPay(removeFee);
            if (!isEnableRemoveMono)
            {
                deleteButton.gameObject.SetActive(false);
                return;
            }

            deleteButton.gameObject.SetActive(true);
        }
    }
}