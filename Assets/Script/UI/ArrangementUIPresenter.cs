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
                GameManager.Instance.ArrangementManager.RemoveSelectArrangement();
                this.Close();
            });

            closeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ArrangementManager.RemoveSelection();
                this.Close();
            });
        }

        public void Show()
        {
            arrangementUI.SetActive(true);
            deleteButton.gameObject.SetActive(GameManager.Instance.ArrangementManager.IsRemoveSelectArrangement());
        }

        public void Close()
        {
            arrangementUI.SetActive(false);
        }
	}
}