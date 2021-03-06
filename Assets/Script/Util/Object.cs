using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    class Object {
        public static Vector3 To2D (Vector3 position) {
            position.y = 0;
            return position;
        }

        public static GameObject Appear (GameObject prefab, GameObject parent, Vector3 position) {
            GameObject instance = GameObject.Instantiate (prefab);
            if (parent != null) {
                instance.transform.SetParent (parent.transform, false);
            }
            instance.transform.position = position;
            return instance;
        }

        public static GameObject AppearToFloor (GameObject prefab, GameObject parent, Vector3 position) {
            GameObject instance = GameObject.Instantiate (prefab);
            if (parent != null) {
                instance.transform.SetParent (parent.transform, false);
            }
            instance.transform.position = Object.To2D (position);
            return instance;
        }

        public static void SetParent (GameObject instance, GameObject parent) {
            instance.transform.SetParent(parent.transform, parent);
        }

        public static GameObject Appear2D (GameObject prefab, GameObject parent, Vector2? position = null) {
            GameObject instance = GameObject.Instantiate (prefab);
            instance.transform.SetParent (parent.transform, false);
            if (position != null) {
                instance.transform.position = position.Value;
            }
            return instance;
        }

        public static GameObject AppearLocal2D (GameObject prefab, GameObject parent, Vector2? position = null) {
            GameObject instance = GameObject.Instantiate (prefab);
            instance.transform.SetParent (parent.transform, false);
            if (position != null) {
                instance.transform.localPosition = position.Value;
            }
            return instance;
        }        

        public static void DisAppear (GameObject instance) {
            GameObject.Destroy (instance);
        }
    }
}