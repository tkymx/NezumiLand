using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class FieldSelect : SelectBase
    {
        private MonoInfo makingMono = null;

        private void Awake()
        {
            // これは後で別から取得できるようにしたい
            makingMono = new MonoInfo()
            {
                Width = 2,
                Height = 2,
                monoPrefab = ResourceLoader.LoadPrefab("Model/branko"),
            };
        }

        public override void OnOver(RaycastHit hit)
        {
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(hit.point, makingMono));
        }

        public override void OnSelect(RaycastHit hit)
        {
            var makingPrefab = ResourceLoader.LoadPrefab("Model/Making");

            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(hit.point, makingMono));
            var currentTarget = GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget();
            if (GameManager.Instance.ArrangementManager.IsSetArrangement(currentTarget))
            {
                // 押した地点に移動して作成する
                if (!GameManager.Instance.Mouse.IsMaking())
                {
                    GameManager.Instance.Mouse.OrderMaking(GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget(), new PreMono(GameManager.Instance.Mouse, makingPrefab, makingMono));
                }
            }
        }
    }
}