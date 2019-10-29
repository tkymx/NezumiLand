using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    class Object
    {
        public static Vector3 To2D(Vector3 position)
        {
            position.y = 0;
            return position;
        }

        public static GameObject AppearToFloor(GameObject prefab,GameObject parent, Vector3 position)
        {
            GameObject instance = GameObject.Instantiate(prefab);
            instance.transform.SetParent(parent.transform, false);
            instance.transform.position = Object.To2D(position);
            return instance;
        }

        public static GameObject Appear2D(GameObject prefab, GameObject parent, Vector3 position)
        {
            GameObject instance = GameObject.Instantiate(prefab);
            instance.transform.SetParent(parent.transform, false);
            instance.transform.position = position;
            return instance;
        }

        public static void DisAppear(GameObject instance)
        {
            GameObject.Destroy(instance);
        }
    }
}