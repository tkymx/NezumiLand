using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class PrivacyPolicyButton : MonoBehaviour
    {
        [SerializeField]
        private Button privacyPolicyButton = null;

        private void Start() {
            privacyPolicyButton.onClick.AddListener(() => {
                string url = "https://github.com/tkymx/WeddingProject/wiki/Privacy-Policy";
                Application.OpenURL(url);
            });
        } 
    }   
}