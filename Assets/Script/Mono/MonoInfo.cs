using UnityEngine;

namespace NL
{
    /// <summary>
    /// モノの情報を持っている
    /// </summary>
    public class MonoInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Currency makingFee { get; set; }
        public GameObject monoPrefab;
    }
}
