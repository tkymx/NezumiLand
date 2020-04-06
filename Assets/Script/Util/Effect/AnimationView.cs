using System;
using UnityEngine;

namespace NL
{
    public class AnimationView : MonoBehaviour
    {
        /// <summary>
        /// アニメーションを再生するレイヤー
        /// </summary>
        private readonly int Baselayer = 0;

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private string playStateName = null;

        private bool isPlayering = false;

        private TypeObservable<int> onComplated;
        public TypeObservable<int> OnComplated => onComplated;

        public void Initialize()
        {
            this.onComplated = new TypeObservable<int>();
            this.isPlayering = false;
        }

        public void StartAnimation()
        {
            // ベースレイヤーから実行
            this.animator.Play(playStateName, Baselayer, 0.0f);
            this.isPlayering = true;
        }

        public void UpdateByFrame()
        {
            if (!this.isPlayering)
            {
                return;
            }

            if (isComplated())
            {
                this.onComplated.Execute(0);
                this.isPlayering = false;
            }            
        }

        private bool isComplated()
        {
            return this.animator.GetCurrentAnimatorStateInfo(Baselayer).shortNameHash != Animator.StringToHash(playStateName);
        }        
    }
}