using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class EffectManager {
        private Camera camera;
        private GameObject root3D;
        private GameObject root2D;

        public EffectManager (Camera camera, GameObject root3D, GameObject root2D) {
            this.camera = camera;
            this.root3D = root3D;
            this.root2D = root2D;
        }

        // Update is called once per frame
        public void UpdateByFrame () {

        }

        public EffectHandlerBase PlayEffect(string prefabName, Vector3 position)
        {
            var prefab = ResourceLoader.LoadModel(prefabName);
            var instance = Object.AppearToFloor(prefab, this.root3D, position);
            var handler = instance.GetComponent<EffectHandlerBase>();
            handler.Initialize();
            return handler;
        }

        public EffectHandlerBase PlayEffect2D(string prefabName, Vector2? position = null)
        {
            var prefab = ResourceLoader.LoadPrefab("UI/prefab/"+prefabName);
            var instance = Object.Appear2D(prefab, this.root2D, position);
            var handler = instance.GetComponent<EffectHandlerBase>();
            handler.Initialize();
            return handler;
        }        

        public void PlayEarnEffect (Currency earn, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/earn_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = earn.Value.ToString () + "yen獲得";
        }

        public void PlayHeartEffect (Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/heart");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
        }

        public void PlayEarnItemEffect (ArrangementItemAmount item, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/earn_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = item.Value.ToString () + "個獲得";
        }        

        public void PlayConsumeEffect (Currency fee, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/consume_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = fee.Value.ToString () + "yen消費";
        }

        public void PlayError (string errorMessage, Vector3 position) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/consume_effect");
            var screenPoint = camera.WorldToScreenPoint (position);
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = errorMessage;
        }

        public void PlayErrorFrom2D (string errorMessage, Vector2 screenPoint) {
            var effectPrefab = ResourceLoader.LoadPrefab ("UI/prefab/consume_effect");
            var instance = Object.Appear2D (effectPrefab, root2D, screenPoint);
            var text = instance.GetComponent<Text> ();
            text.text = errorMessage;
        }
    }
}