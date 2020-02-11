using UnityEngine;

namespace NL
{
    /// <summary>
    /// 時間経過で終了を監視するハンドラー
    /// </summary>
    public class AnimationEffectHandler : EffectHandlerBase {

        /// <summary>
        /// アニメーションを再生するレイヤー
        /// </summary>
        private readonly int Baselayer = 0;

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private string playStateName = null;

        private TypeObservable<int> onComplated;
        public override TypeObservable<int> OnComplated => onComplated;

        public AnimationEffectHandler()
        {
            this.onComplated = new TypeObservable<int>();
        }

        public override void Initialize()
        {
            // ベースレイヤーから実行
            this.animator.Play(playStateName, Baselayer, 0.0f);
        }

        public void Update()
        {
            if (isComplated()){
                this.onComplated.Execute(0);
                this.Remove();
            }            
        }

        private bool isComplated()
        {
            return this.animator.GetCurrentAnimatorStateInfo(Baselayer).shortNameHash != Animator.StringToHash(playStateName);
        }
    }
}