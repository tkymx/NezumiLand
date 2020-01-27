using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class HeartView : MonoBehaviour
    {
        [SerializeField]
        private Slider slider = null;

        [SerializeField]
        private Text heartCountText = null;

        [SerializeField]
        private GameObject goalLine = null;

        public void Initialize() {
        }

        public void UpdateView(int currentHeartCount, int maxHeartCount, int goalHeartCount) {

            var rateCurrent =  (float)currentHeartCount / (float)maxHeartCount;
            var rateGoal =  (float)goalHeartCount / (float)maxHeartCount;

            this.heartCountText.text = currentHeartCount.ToString() + "/" + maxHeartCount.ToString();
            this.slider.value = rateCurrent;

            var sliderRect = this.slider.GetComponent<RectTransform>();
            Debug.Assert(sliderRect != null, "RectTransformがセットされていません");
            var sliderWidth = sliderRect.sizeDelta.x;

            var goalLinePosition = goalLine.transform.position;
            goalLinePosition.x = sliderRect.rect.xMin + sliderWidth * rateGoal;
        }
    }   
}