using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class PreMonoView : MonoBehaviour
    {
        [SerializeField]
        private Slider makingProgressSlider = null;

        public void UpdateView(float rate) {
            makingProgressSlider.value = rate;
        }
    }    
}