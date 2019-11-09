using UnityEngine;
using System.Linq;

namespace NL
{
    /// <summary>
    /// モノの情報を持っている
    /// </summary>
    public class MonoInfo
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Currency MakingFee { get; private set; }
        public Currency RemoveFee { get; private set; }
        public GameObject MonoPrefab { get; private set; }
        public Currency[] LevelUpFee { get; private set; }
        public Satisfaction[] LevelUpSatisfaction { get; private set; }
        public Satisfaction BaseSatisfaction { get; private set; }

        public MonoInfo(
            uint Id,
            string Name,
            int Width,
            int Height,
            long MakingFee,
            long RemoveFee,
            string ModelName,
            long[] LevelUpFee,
            long[] LevelUpSatisfaction,
            long BaseSatisfaction)
        {
            this.Id = Id;
            this.Name = Name;
            this.Width = Width;
            this.Height = Height;
            this.MakingFee = new Currency(MakingFee);
            this.RemoveFee = new Currency(RemoveFee);
            this.MonoPrefab = ResourceLoader.LoadModel(ModelName);
            this.LevelUpFee = LevelUpFee.Select(fee => new Currency(fee)).ToArray();
            this.LevelUpSatisfaction = LevelUpSatisfaction.Select(satisfaction => new Satisfaction(satisfaction)).ToArray();
            this.BaseSatisfaction = new Satisfaction(BaseSatisfaction);
        }
    }
}
