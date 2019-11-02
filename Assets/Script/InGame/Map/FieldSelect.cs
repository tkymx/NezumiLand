using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class FieldSelect : SelectBase
    {
        public override void OnOver(RaycastHit hit)
        {
            if (!GameManager.Instance.ArrangementManager.IsEnable)
            {
                return;
            }

            var makingMono = GameManager.Instance.ArrangementManager.ArrangementMonoInfo;
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(hit.point, makingMono));
        }

        public override void OnSelect(RaycastHit hit)
        {
            if (!GameManager.Instance.ArrangementManager.IsEnable)
            {
                return;
            }

            var makingMono = GameManager.Instance.ArrangementManager.ArrangementMonoInfo;
            var makingPrefab = ResourceLoader.LoadPrefab("Model/Making");

            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate(ArrangementInfoGenerator.Generate(hit.point, makingMono));
            var currentTarget = GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget();
            if (GameManager.Instance.ArrangementManager.IsSetArrangement(currentTarget))
            {
                // 押した地点に移動して作成する
                if (!GameManager.Instance.Mouse.IsOrder())
                {
                    // この辺は販売関連のクラスでまとめたほうが良さげ
                    if (GameManager.Instance.Wallet.IsPay(makingMono.MakingFee))
                    {
                        GameManager.Instance.Wallet.Pay(makingMono.MakingFee);
                        GameManager.Instance.EffectManager.PlayRemoveMonoEffect(makingMono.MakingFee, currentTarget.CenterPosition);
                        GameManager.Instance.Mouse.OrderMaking(currentTarget, new PreMono(GameManager.Instance.Mouse, makingPrefab, makingMono));
                    }
                    else
                    {
                        GameManager.Instance.EffectManager.PlayError("お金がありません。", currentTarget.CenterPosition);
                    }
                }
            }
        }
    }
}