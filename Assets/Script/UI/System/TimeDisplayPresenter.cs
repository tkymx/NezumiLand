using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class TimeDisplayPresenter : MonoBehaviour {

        [SerializeField]
        private Text dayValue = null;

        [SerializeField]
        private Slider progress = null;

        [SerializeField]
        private GameObject NormalBar = null;

        [SerializeField]
        private GameObject StopBar = null;

        // Start is called before the first frame update
        void Start () {
            this.NormalBar.SetActive(true);
            this.StopBar.SetActive(false);
        }

        // Update is called once per frame
        void Update () {
            dayValue.text = GameManager.Instance.TimeManager.ToString();
            progress.value = DayTextConverter.OneDayProgress(GameManager.Instance.TimeManager.ElapsedTime);

            if (GameManager.Instance.TimeManager.IsPause) {
                this.NormalBar.SetActive(false);
                this.StopBar.SetActive(true);
            } else {
                this.NormalBar.SetActive(true);
                this.StopBar.SetActive(false);
            }
        }
    }
}