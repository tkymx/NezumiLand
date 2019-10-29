using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NL
{
    public class ArrangementMode : IGameMode
    {
        private Camera mainCamera;

        public ArrangementMode(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }

        public void OnEnter()
        {

        }

        public void OnUpdate()
        {
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation();
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity, LayerMask.GetMask(new string[] { "SelectLayer", "Floor" })))
            {
                var select = Hit.transform.GetComponent<SelectBase>();
                Debug.Assert(select != null, "選択したオブジェクトに ISelect がありません");

                if (select != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (CanTouch())
                        {
                            select.OnSelect(Hit);
                        }
                    }
                    else
                    {
                        select.OnOver(Hit);
                    }

                }
            }
        }

        public void OnExit()
        {

        }

        private bool CanTouch()
        {
            if (Input.touchCount > 0)
            {
                return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }
            return !EventSystem.current.IsPointerOverGameObject();
        }
    }
}