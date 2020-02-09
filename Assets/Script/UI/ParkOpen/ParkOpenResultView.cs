using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenResultView : MonoBehaviour
    {
        [SerializeField]
        private Text currentHeartCountText = null;

        [SerializeField]
        private Text goalHeartCountText = null;

        [SerializeField]
        private GameObject successNotify = null;

        [SerializeField]
        private GameObject failueNotify = null;

        [SerializeField]
        private Button back = null;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }

        public void UpdateView(string currentHeartCount, string goalHeartCount, bool isSuccess) {
            this.currentHeartCountText.text = currentHeartCount;
            this.goalHeartCountText.text = goalHeartCount;
            this.successNotify.SetActive(isSuccess);
            this.failueNotify.SetActive(!isSuccess);
        }
    }   
}