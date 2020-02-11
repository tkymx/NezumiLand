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

        private TypeObservable<int> onComplated;
        public override TypeObservable<int> OnComplated => onComplated;

        public TimeEffectHandler()
        {
            this.onComplated = new TypeObservable<int>();
        }

        public override void Initialize()
        {
            this.elapsetdTime = 0;
        }

        public void Update()
        {
            if (isComplated()){
                return;
            }
            
            elapsetdTime += GameManager.Instance.TimeManager.DeltaTime();

            // 条件を満たすと一度だけ発火
            if (isComplated()){
                this.onComplated.Execute(0);
            }
        }

        private bool isComplated()
        {
            return timeLimit < elapsetdTime;
        }
    }
}