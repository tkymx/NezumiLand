using UnityEngine;

namespace NL
{
    /// <summary>
    /// エフェクトの終了を監視するハンドラー
    /// </summary>
    public abstract class EffectHandlerBase : MonoBehaviour {
        public abstract bool IsComplated();
        public void Remove()
        {
            Object.DisAppear(gameObject);
        }
    }
}