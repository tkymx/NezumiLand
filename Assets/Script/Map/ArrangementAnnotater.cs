using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementAnnotater {
        public static readonly int ArrangementWidth = 6; // prefabからとってこれるようにする
        public static readonly int ArrangementHeight = 6;

        private GameObject annotationPrefab;

        private ArrangementInfo currentArrangemtnInfo;
        private List<GameObject> currentAnnotation;
        private List<ArrangementPosition> currentArrangementPositions;

        private GameObject objectParent;

        public TypeObservable<int> OnSelect { get; private set;}

        public void Select () {
            this.OnSelect.Execute(0);
        }

        public ArrangementAnnotater (GameObject objectParent) {
            this.OnSelect = new TypeObservable<int>();
            this.objectParent = objectParent;
            this.currentAnnotation = new List<GameObject> ();
            this.currentArrangementPositions = new List<ArrangementPosition> ();

            this.annotationPrefab = ResourceLoader.LoadPrefab ("Model/arrangement_annotation");
        }

        public void Annotate (ArrangementInfo arrangementInfo) {
            this.currentArrangemtnInfo = arrangementInfo;
            currentAnnotation.Clear ();
            currentArrangementPositions.Clear ();

            for (int x = 0; x < arrangementInfo.mono.Width; x++) {
                for (int z = 0; z < arrangementInfo.mono.Height; z++) {
                    var appear = Object.AppearToFloor (annotationPrefab, objectParent, new Vector3 (
                        (arrangementInfo.x + x) * ArrangementWidth,
                        0,
                        (arrangementInfo.z + z) * ArrangementHeight));

                    currentAnnotation.Add (appear);
                    currentArrangementPositions.Add (new ArrangementPosition () {
                        x = arrangementInfo.x + x,
                            z = arrangementInfo.z + z
                    });
                }
            }
        }
        public void RemoveAllAnnotation () {
            this.currentArrangemtnInfo = null;
            foreach (var appearObject in currentAnnotation) {
                Object.DisAppear (appearObject);
            }
            this.currentAnnotation.Clear ();
        }

        public YesPlayerArrangementTarget GetCurrentTarget () {
            if (this.currentArrangemtnInfo == null)
            {
                return null;
            }
            return new YesPlayerArrangementTarget (this.currentAnnotation, currentArrangementPositions, currentArrangemtnInfo);
        }
    }
}