using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class EffectManager {
        private Camera camera;
        private GameObject root;

        public EffectManager (Camera camera, GameObject root) {
            this.camera = camera;
            this.root = root;
        }

        // Update is called once per frame
        public void UpdateByFrame () {

        }

        public void PlayEarnEffect (Currency earn, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/earn_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = earn.Value.ToString () + "yen獲得";
        }

        public void PlayHeartEffect (Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/heart");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
        }

        public void PlayEarnItemEffect (ArrangementItemAmount item, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/earn_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = item.Value.ToString () + "個獲得";
        }        

        public void PlayConsumeEffect (Currency fee, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/consume_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = fee.Value.ToString () + "yen消費";
        }

        public void PlayError (string errorMessage, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/consume_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = errorMessage;
        }

        public void PlayErrorFrom2D (string errorMessage, Vector2 screenPoint) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/consume_effect");
            var instance = Object.Appear2D (effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = errorMessage;
        }
    }
}