using UnityEngine;
using System.Linq;
using System;

namespace NL
{
    public enum MonoType
    {
        None,
        Equipment,
        Tool
    }

    /// <summary>
    /// モノの情報を持っている
    /// </summary>
    public class MonoInfo
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public MonoType Type { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Currency MakingFee { get; private set; }
        public Currency RemoveFee { get; private set; }
        public GameObject MonoPrefab { get; private set; }
        public Currency[] LevelUpFee { get; private set; }
        public Satisfaction[] LevelUpSatisfaction { get; private set; }
        public Satisfaction BaseSatisfaction { get; private set; }
        public ArrangementItemAmount ArrangementItemAmount { get; private set; }
        public ArrangementCount ArrangementCount { get; private set; }

        public MonoInfo(
            uint Id,
            string Name,
            string Type,
            int Width,
            int Height,
            long MakingFee,
            long MakingItemAmount,
            long RemoveFee,
            string ModelName,
            long[] LevelUpFee,
            long[] LevelUpSatisfaction,
            long BaseSatisfaction,
            long ArrangementCount)
        {
            this.Id = Id;
            this.Name = Name;

            this.Type = MonoType.None;
            if (Enum.TryParse(Type, out MonoType outMonoType))
            {
                this.Type = outMonoType;
            }

            this.Width = Width;
            this.Height = Height;
            this.MakingFee = new Currency(MakingFee);
            this.ArrangementItemAmount = new ArrangementItemAmount(MakingItemAmount);
            this.RemoveFee = new Currency(RemoveFee);
            this.MonoPrefab = ResourceLoader.LoadModel(ModelName);
            this.LevelUpFee = LevelUpFee.Select(fee => new Currency(fee)).ToArray();
            this.LevelUpSatisfaction = LevelUpSatisfaction.Select(satisfaction => new Satisfaction(satisfaction)).ToArray();
            this.BaseSatisfaction = new Satisfaction(BaseSatisfaction);
            this.ArrangementCount = new ArrangementCount(this.Id, ArrangementCount);
        }

        public ArrangementResourceAmount ArrangementResourceAmount
        {
            get
            {
                return new ArrangementResourceAmount(this.MakingFee, this.ArrangementItemAmount, this.ArrangementCount);
            }
        }
    }
}
