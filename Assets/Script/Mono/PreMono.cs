using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NL
{
    public class PreMono
    {
        private Mouse mouse;
        private GameObject makingPrefab;
        private GameObject monoPrefab;

        private GameObject makingInstane;

        public PreMono(Mouse mouse, GameObject makingPrefab, GameObject monoPrefab)
        {
            this.mouse = mouse;
            this.makingPrefab = makingPrefab;
            this.monoPrefab = monoPrefab;
        }

        public void StartMaking(Vector3 position)
        {
            this.makingInstane = Object.Appear(makingPrefab, mouse.transform.parent.gameObject, position);
        }

        public void FinishMaking(Vector3 position)
        {
            Object.DisAppear(this.makingInstane);
            Object.Appear(monoPrefab, mouse.transform.parent.gameObject, position);
        }
    }
}
