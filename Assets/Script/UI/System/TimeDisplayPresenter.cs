using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class TimeDisplayPresenter : MonoBehaviour
    {
        [SerializeField]
        private Text remainValue = null;

        [SerializeField]
        private Text dayValue = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            remainValue.text = GameManager.Instance.DailyActionManager.RemainOverSecond.ToString("F0");
            dayValue.text = GameManager.Instance.DailyActionManager.DaySecond.ToString("F0");
        }
    }
}