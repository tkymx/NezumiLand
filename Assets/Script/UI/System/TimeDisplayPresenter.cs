using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class TimeDisplayPresenter : MonoBehaviour
    {
        [SerializeField]
        private Text timeValue = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            timeValue.text = GameManager.Instance.TimeManager.ElapsedTime.ToString("F0");
        }
    }
}