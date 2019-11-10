using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ArrangementItemPresenter : MonoBehaviour
    {
        [SerializeField]
        Text text = null;

        void Update()
        {
            text.text = GameManager.Instance.ArrangementItemStore.CurrentArrangementItemAmount.ToString();
        }
    }
}

