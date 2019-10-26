using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class TestuserInputPresenter
    {
        private Camera mainCamera;
        private Mouse mouse;
        public MonoInfo makingMono;

        private ArrangementAnnotater arrangementAnnotater;
        public GameObject makingPrefab;

        public TestuserInputPresenter(Camera mainCamera, Mouse mouse, MonoInfo makingMono, GameObject rootObject)
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
            arrangementAnnotater.RemoveAllAnnotation();
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity, LayerMask.GetMask(new string[]{"SelectLayer","Floor"})))
            {
                var arrangementView = Hit.transform.GetComponent<ArrangementView>();
                if (arrangementView != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        arrangementView.OnSelect.Execute(0);
                    }
                }
                else
                {
                    arrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(Hit.point, makingMono));

                    if (Input.GetMouseButtonDown(0))
                    {
                        var currentTarget = arrangementAnnotater.GetCurrentTarget();
                        if (GameManager.Instance.ArrangementManager.IsSetArrangement(currentTarget))
                        {
                            // 押した地点に移動して作成する
                            if (!mouse.IsMaking())
                            {
                                mouse.OrderMaking(arrangementAnnotater.GetCurrentTarget(), new PreMono(mouse, makingPrefab, makingMono));
                            }
                        }
                    }
                }
            }
        }
    }
}
