using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenManager : IDisposable {

        private readonly IConversationRepository conversationRepository = null;
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        private IParkOpenDirector parkOpenDirector = null;
        private List<IDisposable> disposables = null;

        /// <summary>
        /// 現在の時間の進行状況
        /// </summary>
        public ParkOpenTimeAmount CurrentParkOpenTimeAmount => this.parkOpenDirector.CurrentParkOpenTimeAmount;

        /// <summary>
        /// 遊園地開放の終了
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenGroupModel> OnCompleted { get; private set; }

        private ParkOpenComment parkOpenComment = null;

        public ParkOpenManager(IConversationRepository conversationRepository, IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.disposables = new List<IDisposable>();
            this.parkOpenDirector = new NopParkOpenDirector(playerParkOpenRepository);
            this.OnCompleted = new TypeObservable<ParkOpenGroupModel>();
            this.playerParkOpenRepository = playerParkOpenRepository;
            this.parkOpenComment = new ParkOpenComment(conversationRepository);
        }

        public void Open(PlayerParkOpenGroupModel playerParkOpenGroupModel)
        {
            var parkOpenDirector = new ParkOpenDirector(playerParkOpenGroupModel, playerParkOpenRepository);
            Open(parkOpenDirector, playerParkOpenGroupModel, true);
        }

        public void Open(PlayerParkOpenModel playerParkOpenModel)
        {
            var parkOpenDirector = new ParkOpenDirector(playerParkOpenModel, playerParkOpenRepository);
            Open(parkOpenDirector, playerParkOpenModel.PlayerParkOpenGroupModel, false);
        }

        private void Open(IParkOpenDirector parkOpenDirector, PlayerParkOpenGroupModel playerParkOpenGroupModel, bool isInitial)
        {
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");

            //モードを更新
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateParkOpenMode());

            // 開放をオープンする
            Action startParkOpen = () => {
                this.parkOpenDirector = parkOpenDirector;
                this.SetEventInternal(playerParkOpenGroupModel);
                this.parkOpenDirector.UpdateParkOpenInfo();
            };

            // 開始コメント
            if (isInitial)
            {
                this.disposables.Add(this.parkOpenComment.StartInitialComment()
                    .SelectMany(_ => {
                        // 開始エフェクト
                        var effectHandler = GameManager.Instance.EffectManager.PlayEffect2D("ParkOpenStartEffect");
                        return effectHandler.OnComplated;
                    })
                    .Subscribe(_ => {
                        startParkOpen();
                    }));
            }
            else
            {
                startParkOpen();
            }

        }

        /// <summary>
        /// ハートの追加を行う
        /// </summary>
        /// <param name="increaseCount"></param>
        public void AddHeart(int increaseCount)
        {
            this.parkOpenDirector.AddHeart(increaseCount);
        }
        
        /// <summary>
        /// 遊び場公開のイベント周りの設定
        /// </summary>
        /// <param name="parkOpenGroupModel"></param>
        private void SetEventInternal(PlayerParkOpenGroupModel playerParkOpenGroupModel)
        {
            this.ClearDisposables();

            // ウェーブが開始した時
            this.disposables.Add(this.parkOpenDirector.OnStartWave.Subscribe(parkOpenWaveModel => {
                GameManager.Instance.ParkOpenAppearManager.AppearWave(parkOpenWaveModel);
                this.parkOpenDirector.UpdateParkOpenInfo();
            }));

            // 開放が終了した時
            this.disposables.Add(this.parkOpenDirector.OnCompleted
                .SelectMany(parkOpenResultAmount => {
                    return GameManager.Instance.EffectManager.PlayEffect2D("ParkOpenFinishEffect")
                        .OnComplated
                        .Select(_ => parkOpenResultAmount);
                })
                .Do(parkOpenResultAmount => {
                    // 結果画面を表示する
                    GameManager.Instance.GameUIManager.ParkOpenResultPresenter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenResultPresenter.SetContents(parkOpenResultAmount);
                })
                .SelectMany(parkOpenResultAmount => {
                    // 結果表示終了を待つ
                    return GameManager.Instance.GameUIManager.ParkOpenResultPresenter
                        .OnClose
                        .Select(_ => parkOpenResultAmount);
                })
                .SelectMany<ParkOpenResultAmount, int>(parkOpenResultAmount => {
                    
                    // 以下はトランザクションにしたい。。。。。

                    // 終了とする
                    this.parkOpenDirector = new NopParkOpenDirector(this.playerParkOpenRepository);
                    this.parkOpenDirector.UpdateParkOpenInfo();

                    // 次のクエストの状態のロットを行う
                    GameManager.Instance.ParkOpenGroupManager.LotParkOpenGroup();

                    // 結果のフラグを立てる
                    if (parkOpenResultAmount.IsSuccess) {
                        GameManager.Instance.ParkOpenGroupManager.ToClearGroup(parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel);
                    }

                    if (!parkOpenResultAmount.IsSuccess) {
                        return new ImmediatelyObservable<int>(0);
                    }

                    // 成功していたらイベントを進める
                    GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.ClearParkOpenGroup(parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel));

                    // 成功していたら報酬を受け取る
                    var rewardReceiver = new RewardReceiver(this.FetchAquirableReward(parkOpenResultAmount));
                    rewardReceiver.ReceiveRewardAndShowModel();                    
                    return rewardReceiver.OnEndReceiveObservable;
                })
                .Subscribe(_ => {
                    // 終了
                    this.OnCompleted.Execute(playerParkOpenGroupModel.ParkOpenGroupModel);
                }));
        }

        /// <summary>
        /// 各報酬の集計を行う
        /// </summary>
        /// <param name="parkOpenResultAmount"></param>
        /// <returns></returns>
        private List<IRewardAmount> FetchAquirableReward(ParkOpenResultAmount parkOpenResultAmount)
        {
            List<IRewardAmount> rewardAmounts = new List<IRewardAmount>();

            // 通常報酬
            rewardAmounts.AddRange(parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel.ClearReward.RewardAmounts);

            // 初回クリア報酬
            if (parkOpenResultAmount.IsFirstClear) {
                rewardAmounts.AddRange(parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel.FirstClearReward.RewardAmounts);
            }

            // 特別報酬
            if (parkOpenResultAmount.TargetPlayerGroupModel.IsSpecial) {
                rewardAmounts.AddRange(parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel.SpecialClearReward.RewardAmounts);                                
            }

            // 成果報酬
            foreach (var specialRewardResult in parkOpenResultAmount.SpecialRewardResults)
            {
                if (specialRewardResult.IsClear) {
                    rewardAmounts.AddRange(specialRewardResult.ParkOpenHeartRewardAmount.Reward.RewardAmounts);
                }
            }

            return rewardAmounts;
        }

        private float elapsedTime = 0;

        public void UpdateByFrame()
        {
            // 一定時間ごとに開放の状況を記録する
            this.elapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (elapsedTime > 1.0f) {
                this.parkOpenDirector.UpdateParkOpenInfo();
                this.elapsedTime = 0;
            }

            this.parkOpenDirector.UpdateByFrame();
        }        

        public void ClearDisposables()
        {
            this.disposables.ForEach(disposable => {
                disposable.Dispose();
            });
            this.disposables.Clear();
        }

        public void Dispose()
        {
            this.ClearDisposables();
            this.parkOpenDirector.Dispose();            
        }

        public override string ToString()
        {
            return this.parkOpenDirector.ToString();
        }
    }    
}