using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NL
{
    public class TestuserInputPresenter
    {
        private Camera mainCamera;

        public TestuserInputPresenter(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }

        // Update is called once per frame
        public void UpdateByFrame()
        {
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation();
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity, LayerMask.GetMask(new string[]{"SelectLayer","Floor"})))
            {
                var select = Hit.transform.GetComponent<SelectBase>();
                Debug.Assert(select != null, "選択したオブジェクトに ISelect がありません");

                if (select != null)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            select.OnSelect(Hit);
                        }
                        else
                        {
                            select.OnOver(Hit);
                        }
                    }
                }
            }
        }
    }
}
