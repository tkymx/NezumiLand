using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// カードが実行されたときの実行処理を行う
    /// 実行タイミング
    /// - カードを使った時
    /// 終了タイミング
    /// - 実行が終わった時
    /// </summary>
    public class ParkOpenActionExecuter
    {
        private List<ParkOpenCardAction.IParkOpenCardAction> parkOpenCardActions;
        private List<ParkOpenCardAction.IParkOpenCardAction> removableCardActions;

        /// <summary>
        /// 実行が完了したときのハンドル
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenCardModel> OnComplate { get; private set; }

        public ParkOpenActionExecuter()
        {
            this.OnComplate = new TypeObservable<ParkOpenCardModel>();
            this.parkOpenCardActions = new List<ParkOpenCardAction.IParkOpenCardAction>();
            this.removableCardActions = new List<ParkOpenCardAction.IParkOpenCardAction>();
        }

        private ParkOpenCardAction.IParkOpenCardAction GenerateParkOpenCardAction(ParkOpenCardActionType parkOpenCardActionType)
        {
            switch (parkOpenCardActionType)
            {
                case ParkOpenCardActionType.CountToHeart:
                {
                    return new ParkOpenCardAction.CountToHeart();
                }
                default:
                {
                    Debug.Assert(false, "登録されていないタイプです。");
                    break;
                }
            }
            return new ParkOpenCardAction.Empty();
        }

        public void Start(ParkOpenCardModel parkOpenCardModel)
        {
            var parkOpenCardAction = GenerateParkOpenCardAction(parkOpenCardModel.ParkOpenCardActionModel.ParkOpenCardActionType);
            parkOpenCardAction.OnStart(parkOpenCardModel);
            this.parkOpenCardActions.Add(parkOpenCardAction);
        }

        public void UpdateByFrame()
        {
            this.removableCardActions.Clear();
            foreach (var parkOpenCardAction in this.parkOpenCardActions)
            {
                parkOpenCardAction.OnUpdate();
                if (!parkOpenCardAction.IsAlive())
                {
                    this.removableCardActions.Add(parkOpenCardAction);
                    parkOpenCardAction.OnComplate();
                    OnComplate.Execute(parkOpenCardAction.ParkOpenCardModel);
                }
            }
            foreach (var removableCardAction in this.removableCardActions)
            {
                this.parkOpenCardActions.Remove(removableCardAction);
            }
        }
    }    
}

