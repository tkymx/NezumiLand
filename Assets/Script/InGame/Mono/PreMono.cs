using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NL
{
    public class PreMono
    {
        private Mouse mouse;
        private GameObject makingPrefab;
        private MonoInfo mono;

        private GameObject makingInstane;

        public PreMono(Mouse mouse, GameObject makingPrefab, MonoInfo mono)
        {
            this.mouse = mouse;
            this.makingPrefab = makingPrefab;
            this.mono = mono;
        }

        public void StartMaking(IArrangementTarget arrangementTarget)
        {
            this.makingInstane = Object.AppearToFloor(makingPrefab, mouse.transform.parent.gameObject, arrangementTarget.CenterPosition);
        }

        public void FinishMaking(IArrangementTarget arrangementTarget)
        {
            Debug.Assert(!arrangementTarget.HasMonoViewModel, "モノがセットされています。");

            Object.DisAppear(this.makingInstane);
            arrangementTarget.MonoViewModel = GameManager.Instance.MonoManager.CreateMono(mono, arrangementTarget.CenterPosition);
        }
    }
}
