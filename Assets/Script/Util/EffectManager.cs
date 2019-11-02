using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class EffectManager
    {
        private Camera camera;
        private GameObject root;

        public EffectManager(Camera camera, GameObject root)
        {
            this.camera = camera;
            this.root = root;
        }

        // Update is called once per frame
        public void UpdateByFrame()
        {

        }

        public void PlayEarnEffect(Currency earn, Vector3 position)
        {
            var effectPrefab = ResourceLoader.LoadPrefab("UI/earn_effect");
            var screenPoint = camera.WorldToScreenPoint(position);
            var instance = Object.Appear2D(effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text>();
            text.text = earn.Value.ToString() + "yen";
        }

        public void PlayRemoveMonoEffect(Currency fee, Vector3 position)
        {
            var effectPrefab = ResourceLoader.LoadPrefab("UI/remove_mono_effect");
            var screenPoint = camera.WorldToScreenPoint(position);
            var instance = Object.Appear2D(effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text>();
            text.text = fee.Value.ToString() + "yen";
        }

        public void PlayError(string errorMessage, Vector3 position)
        {
            var effectPrefab = ResourceLoader.LoadPrefab("UI/remove_mono_effect");
            var screenPoint = camera.WorldToScreenPoint(position);
            var instance = Object.Appear2D(effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text>();
            text.text = errorMessage;
        }

        public void PlayErrorFrom2D(string errorMessage, Vector2 screenPoint)
        {
            var effectPrefab = ResourceLoader.LoadPrefab("UI/remove_mono_effect");
            var instance = Object.Appear2D(effectPrefab, root, screenPoint);
            var text = instance.GetComponent<Text>();
            text.text = errorMessage;
        }
    }
}
