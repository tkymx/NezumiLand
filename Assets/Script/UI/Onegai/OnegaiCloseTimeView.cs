using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiCloseTimeView : MonoBehaviour {

        [SerializeField]
        private Text closeTimeText = null;

        private float closeTime = 0;

        public void UpdateView (bool isCloseTIme, float closeTime) {
            this.closeTimeText.gameObject.SetActive(isCloseTIme);
            this.closeTime = closeTime;
        }

        private void Update() {
            var restTime = this.closeTime - GameManager.Instance.TimeManager.ElapsedTime;
            this.closeTimeText.text = "終了まで：" + Mathf.CeilToInt(restTime).ToString();
        }
    }
}