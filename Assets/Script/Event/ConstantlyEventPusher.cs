using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {

    /// <summary>
    /// 常に関しが必要なイベントに関してプッシュしています。
    /// TODO : 若干重い気もするのでトリガータイプにしたい。。。
    /// </summary>
    public class ConstantlyEventPusher
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;
        private readonly SatisfactionCalculater satisfactionCalculater = null;
        private OnegaiMediater onegaiMediater = null;
        public ConstantlyEventPusher(IPlayerOnegaiRepository playerOnegaiRepository)
        {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.satisfactionCalculater = new SatisfactionCalculater(playerOnegaiRepository);
            this.onegaiMediater = new OnegaiMediater(playerOnegaiRepository);
        }
        public void PushConstantlyEventParameter() {
            var currentSatisfaction = this.satisfactionCalculater.CalcFieldSatisfaction();
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.Time(GameManager.Instance.TimeManager.ElapsedTime));
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.AboveSatisfaction(currentSatisfaction));

            // 通常の判断
            this.onegaiMediater.Mediate (
                new NL.OnegaiConditions.AboveSatisfaction(currentSatisfaction),
                playerOnegaiRepository.GetAll().ToList()
            );
        }
    }
}

