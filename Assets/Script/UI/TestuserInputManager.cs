using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class TestuserInputManager
    {
        private Camera mainCamera;
        private ArrangementAnnotater arrangementAnnotater;

        public TestuserInputManager(Camera mainCamera, GameObject rootObject)
        {
            this.mainCamera = mainCamera;
            this.arrangementAnnotater = new ArrangementAnnotater(rootObject);
        }

        // Update is called once per frame
        public void UpdateByFrame()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {
                    Debug.Log(Hit.transform.gameObject.name);//デバッグログにヒットした場所を出す

                    var arrangementX = Mathf.FloorToInt(Hit.point.x / ArrangementAnnotater.ArrangementWidth) * ArrangementAnnotater.ArrangementWidth;
                    var arrangementZ = Mathf.FloorToInt(Hit.point.z / ArrangementAnnotater.ArrangementHeight) * ArrangementAnnotater.ArrangementHeight;

                    arrangementAnnotater.RemoveAllAnnotation();
                    arrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(arrangementX, arrangementZ));
                }
            }
        }
    }
}
