using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {

    public struct ParkOpenResultAmount {

        public struct SpecialRewardResult
        {
            public ParkOpenRewardAmount ParkOpenRewardAmount { get; }
            public bool IsClear { get; }

            public SpecialRewardResult(
                ParkOpenRewardAmount parkOpenRewardAmount,
                int currentHeartCount
            ) {
                this.ParkOpenRewardAmount = parkOpenRewardAmount;
                this.IsClear = parkOpenRewardAmount.ObtainHeartCount <= currentHeartCount;
            }
        }

        private ParkOpenGroupModel targetGroupModel;
        public ParkOpenGroupModel TargetGroupModel => targetGroupModel;

        private int currentHeartCount;
        public int CurrentHeartCount => currentHeartCount;

        private int goalHeartCount;
        public int GoalHeartCount => goalHeartCount;

        private List<SpecialRewardResult> specialRewardResults;
        public List<SpecialRewardResult> SpecialRewardResults => specialRewardResults;

        public bool IsSuccess {
            get {
                Debug.Assert(this.goalHeartCount > 0, "ゴールが設定されていません。" );
                return this.currentHeartCount >= this.goalHeartCount;
            }
        }

        public ParkOpenResultAmount (ParkOpenGroupModel targetGroupModel, int currentHeartCount, int goalHeartCount, List<ParkOpenRewardAmount> parkOpenRewardAmounts) {
            this.targetGroupModel = targetGroupModel;
            this.currentHeartCount = currentHeartCount;
            this.goalHeartCount = goalHeartCount;
            this.specialRewardResults = parkOpenRewardAmounts
                .Select(parkOpenRewardAmount => {
                    return new SpecialRewardResult(parkOpenRewardAmount, currentHeartCount);
                })
                .ToList();
        }
    }
}