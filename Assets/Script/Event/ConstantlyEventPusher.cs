using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// 常に関しが必要なイベントに関してプッシュしています。
    /// TODO : 若干重い気もするのでトリガータイプにしたい。。。
    /// </summary>
    public class ConstantlyEventPusher
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;
        private readonly SatisfactionCalculater satisfactionCalculater = null;
        public ConstantlyEventPusher(IPlayerOnegaiRepository playerOnegaiRepository)
        {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.satisfactionCalculater = new SatisfactionCalculater(playerOnegaiRepository);
        }
        public void PushConstantlyEventParameter() {
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.Time(GameManager.Instance.TimeManager.ElapsedTime));
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.AboveSatisfaction(this.satisfactionCalculater.CalcFieldSatisfaction()));
        }
    }
}

