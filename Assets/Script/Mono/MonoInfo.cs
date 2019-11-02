using UnityEngine;

namespace NL
{
    /// <summary>
    /// モノの情報を持っている
    /// </summary>
    public class MonoInfo
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Currency MakingFee { get; set; }
        public Currency RemoveFee { get; set; }
        public GameObject MonoPrefab { get; set; }
        public Currency[] LevelUpFee { get; set; }
        public Currency[] LevelUpEarn { get; set; }
    }
}
