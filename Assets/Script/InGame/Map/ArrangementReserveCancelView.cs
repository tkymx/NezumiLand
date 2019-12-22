using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ArrangementReserveCancelView : MonoBehaviour
    {
        [SerializeField]
        private Button onCancel = null;

        [SerializeField]
        private Canvas mainCanvas = null;

        public TypeObservable<int> OnCancelObservable { get; private set; } 

        public void Initialize (Camera mainCamera) {
            this.mainCanvas.worldCamera = mainCamera;
            this.OnCancelObservable = new TypeObservable<int>();
            this.onCancel.onClick.AddListener(() => {
                this.OnCancelObservable.Execute(0);
            });
        }
    }   
}
