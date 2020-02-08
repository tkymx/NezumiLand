using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    namespace ParkOpenCardAction
    {
        /// <summary>
        /// 人数分だけハートをもらう効果
        /// </summary>
        public class CountToHeart : ParkOpenCardAction.IParkOpenCardAction
        {
            private ParkOpenCardModel parkOpenCardModel;
            public ParkOpenCardModel ParkOpenCardModel => parkOpenCardModel;

            private EffectHandlerBase effectHandler = null;

            public void OnStart(ParkOpenCardModel parkOpenCardModel)
            {
                this.parkOpenCardModel = parkOpenCardModel;

                // 倍率
                float rate = float.Parse(this.parkOpenCardModel.ParkOpenCardActionModel.Args[0]);
                string prefabName = this.parkOpenCardModel.ParkOpenCardActionModel.Args[1];
                Vector3 position = new Vector3(
                    float.Parse(this.parkOpenCardModel.ParkOpenCardActionModel.Args[2]),
                    0,
                    float.Parse(this.parkOpenCardModel.ParkOpenCardActionModel.Args[3])
                );

                // ハートを増やす
                var heartCount = GameManager.Instance.AppearCharacterManager.ParkOpenCharacterCount * rate;
                GameManager.Instance.ParkOpenManager.AddHeart((int)heartCount);

                // エフェクトを出す
                this.effectHandler = GameManager.Instance.EffectManager.PlayEffect(prefabName, position);
            }
            public void OnUpdate()
            {
            }
            public bool IsAlive()
            {
                return !this.effectHandler.IsComplated();
            }
            public void OnComplate()
            {
                this.effectHandler.Remove();
            }
        }
    }
}

