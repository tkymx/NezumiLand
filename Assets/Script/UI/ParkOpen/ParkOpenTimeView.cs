using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenTimeView : MonoBehaviour
    {
        [SerializeField]
        private Text elapsedTime = null;

        [SerializeField]
        private Slider timeSlider = null;

        public void UpdateView(string time, float rate) {
            this.elapsedTime.text = time;
            this.timeSlider.value = rate;
        }
    }   
}