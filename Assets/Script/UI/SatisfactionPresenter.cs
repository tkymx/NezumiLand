using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class SatisfactionPresenter : MonoBehaviour
    {
        [SerializeField]
        Text text = null;

        private void Update()
        {
            text.text = SatisfactionCalculater.CalcFieldSatisfaction().ToString();
        }
    }
}


