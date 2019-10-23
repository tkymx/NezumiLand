using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class TestuserInputManager
    {
        private Camera mainCamera;
        private Mouse mouse;
        public Mono makingMono;

        private ArrangementAnnotater arrangementAnnotater;
        public GameObject makingPrefab;


        public TestuserInputManager(Camera mainCamera, Mouse mouse, Mono makingMono, GameObject rootObject)
        {
            this.mainCamera = mainCamera;
            this.mouse = mouse;
            this.makingMono = makingMono;

            this.arrangementAnnotater = new ArrangementAnnotater(rootObject);
            this.makingPrefab = ResourceLoader.LoadPrefab("Model/Making");
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
                    arrangementAnnotater.RemoveAllAnnotation();
                    arrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(Hit.point, makingMono));

                    // 押した地点に移動して作成する
                    mouse.OrderMaking(arrangementAnnotater.GetCurrentTarget(), new PreMono(mouse, makingPrefab, makingMono));
                }
            }
        }
    }
}
