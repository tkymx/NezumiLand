using UnityEngine;

namespace NL
{
    /// <summary>
    /// エフェクトの終了を監視するハンドラー
    /// </summary>
    public abstract class EffectHandlerBase : MonoBehaviour {
        public abstract TypeObservable<int> OnComplated { get; }
        public abstract void Initialize();
        public void Remove()
        {
            Object.DisAppear(gameObject);
        }
    }
}