using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenManager : IDisposable {

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

        public ParkOpenManager(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.disposables = new List<IDisposable>();
            this.parkOpenDirector = new NopParkOpenDirector(playerParkOpenRepository);
            this.OnCompleted = new TypeObservable<ParkOpenGroupModel>();
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void Open(ParkOpenGroupModel parkOpenGroupModel)
        {
            var parkOpenDirector = new ParkOpenDirector(parkOpenGroupModel, playerParkOpenRepository);
            Open(parkOpenDirector, parkOpenGroupModel);
        }

        public void Open(PlayerParkOpenModel playerParkOpenModel)
        {
            var parkOpenDirector = new ParkOpenDirector(playerParkOpenModel, playerParkOpenRepository);
            Open(parkOpenDirector, playerParkOpenModel.ParkOpenGroupModel);
        }

        private void Open(IParkOpenDirector parkOpenDirector, ParkOpenGroupModel parkOpenGroupModel)
        {
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");

            //モードを更新
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateParkOpenMode());

            // エフェクトのあとにスタート
            this.disposables.Add(GameManager.Instance.EffectManager.PlayEffect2D("ParkOpenStartEffect").OnComplated.Subscribe(_ => {
                this.parkOpenDirector = parkOpenDirector;
                this.SetEventInternal(parkOpenGroupModel);
                this.parkOpenDirector.UpdateParkOpenInfo();
            }));
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
        private void SetEventInternal(ParkOpenGroupModel parkOpenGroupModel)
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
                    if (!parkOpenResultAmount.IsSuccess) {
                        return new ImmediatelyObservable<int>(0);
                    }
                    
                    // 以下はトランザクションにしたい。。。。。
                    
                    // 終了とする
                    this.parkOpenDirector = new NopParkOpenDirector(this.playerParkOpenRepository);
                    this.parkOpenDirector.UpdateParkOpenInfo();
                    
                    // 結果のフラグを立てる
                    if (parkOpenResultAmount.IsSuccess) {
                        GameManager.Instance.ParkOpenGroupManager.ClearGroup(parkOpenResultAmount.TargetGroupModel);
                    }

                    // 次のクエストの状態のロットを行う
                    GameManager.Instance.ParkOpenGroupManager.LotParkOpenGroup();

                    // 報酬の受け取り
                    var rewardReceiver = new RewardReceiver(parkOpenResultAmount.TargetGroupModel.ClearReward);
                    rewardReceiver.ReceiveRewardAndShowModel();
                    return rewardReceiver.OnEndReceiveObservable;
                })
                .Subscribe(_ => {
                    // 終了
                    this.OnCompleted.Execute(parkOpenGroupModel);
                }));
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