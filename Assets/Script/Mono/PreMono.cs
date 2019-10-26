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

        public void StartMaking(Vector3 position)
        {
            this.makingInstane = Object.Appear(makingPrefab, mouse.transform.parent.gameObject, position);
        }

        public void FinishMaking(Vector3 position)
        {
            Object.DisAppear(this.makingInstane);
            Object.Appear(mono.monoPrefab, mouse.transform.parent.gameObject, position);
        }
    }
}
