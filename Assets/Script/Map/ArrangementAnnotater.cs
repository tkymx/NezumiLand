using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementAnnotater
    {
        public static readonly int ArrangementWidth = 6; // prefabからとってこれるようにする
        public static readonly int ArrangementHeight = 6;

        private GameObject annotationPrefab;
        private List<GameObject> currentAnnotation;
        private GameObject objectParent;

        public ArrangementAnnotater(GameObject objectParent)
        {
            this.objectParent = objectParent;
            this.currentAnnotation = new List<GameObject>();
            annotationPrefab = ResourceLoader.LoadPrefab("Model/arrangement_annotation");
        }

        public void Annotate(ArrangementInfo arrangementInfo)
        {
            currentAnnotation.Clear();
            int offsetX = 0;// arrangementInfo.mono.Width * ArrangementWidth / 2;
            int offsetZ = 0;// arrangementInfo.mono.Height * ArrangementHeight / 2;
            for (int x = 0; x < arrangementInfo.mono.Width; x++ )
            {
                for (int z = 0; z < arrangementInfo.mono.Height; z++)
                {
                    var appear = Object.Appear(annotationPrefab, objectParent, new Vector3(
                        arrangementInfo.x + x * ArrangementWidth - offsetX,
                        0,
                        arrangementInfo.z + z * ArrangementHeight - offsetZ));

                    currentAnnotation.Add(appear);
                }
            }
        }
        public void RemoveAllAnnotation()
        {
            foreach(var appearObject in currentAnnotation)
            {
                Object.DisAppear(appearObject);
            }
            currentAnnotation.Clear();
        }
    }
}