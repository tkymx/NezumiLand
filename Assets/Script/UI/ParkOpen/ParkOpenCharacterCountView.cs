using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenCharacterCountView : MonoBehaviour
    {
        [SerializeField]
        private Text countText = null;

        public void UpdateView(string count) {
            this.countText.text = count;
        }
    }   
}