using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class OnegaiConditionNotificationView : MonoBehaviour
    {

        [SerializeField]
        private Text onegaiTitle = null;

        [SerializeField]
        private Text satisfactionText = null;

        [SerializeField]
        private GameObject clearLabel = null;
        
        [SerializeField]
        private GameObject failueLabel = null;
        
        [SerializeField]
        private Button closeButton = null;

        public TypeObservable<int> OnCloseObservable { get; private set; }

        public void Initialize () {
            this.OnCloseObservable = new TypeObservable<int>();
            this.closeButton.onClick.AddListener(() => {
                this.OnCloseObservable.Execute(0);
            });
        }

        public void UpdateView (string title, string satisfaction , OnegaiConditionNotificationState state) {
            this.onegaiTitle.text = title;
            this.satisfactionText.text = satisfaction;

            // 表示切り替え
            this.clearLabel.SetActive(false);
            this.failueLabel.SetActive(false);
            switch (state) {
                case OnegaiConditionNotificationState.Clear: {
                    this.clearLabel.SetActive(true);
                    break;
                }
                case OnegaiConditionNotificationState.Failue: {
                    this.failueLabel.SetActive(true);
                    break;
                }
                default : {
                    break;
                }
            }
        }
    }   
}