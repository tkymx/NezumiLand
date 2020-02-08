using UnityEngine;

namespace NL
{
    /// <summary>
    /// 時間経過で終了を監視するハンドラー
    /// </summary>
    public class TimeEffectHandler : EffectHandlerBase {     

        [SerializeField]
        private float timeLimit = 0;

        private float elapsetdTime = 0;

        public void Update()
        {
            if (IsComplated()){
                return;
            }
            
            elapsetdTime += GameManager.Instance.TimeManager.DeltaTime();
        }

        public override bool IsComplated()
        {
            return timeLimit < elapsetdTime;
        }
    }
}