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
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");
            this.parkOpenDirector = new ParkOpenDirector(parkOpenGroupModel, playerParkOpenRepository);
            this.SetEventInternal(parkOpenGroupModel);
            this.parkOpenDirector.UpdateParkOpenInfo();            
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateParkOpenMode());
        }

        public void Open(PlayerParkOpenModel playerParkOpenModel)
        {
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");
            this.parkOpenDirector = new ParkOpenDirector(playerParkOpenModel, playerParkOpenRepository);
            this.SetEventInternal(playerParkOpenModel.ParkOpenGroupModel);
            this.parkOpenDirector.UpdateParkOpenInfo();
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateParkOpenMode());
        }

        /// <summary>
        /// ハートの追加を行う
        /// </summary>
        /// <param name="increaseCount"></param>
        public void AddHeart(int increaseCount)
        {
            this.parkOpenDirector.AddHeart(increaseCount);
        }
        
        private void SetEventInternal(ParkOpenGroupModel parkOpenGroupModel)
        {
            this.ClearDisposables();

            // ウェーブが開始した時
            this.disposables.Add(this.parkOpenDirector.OnStartWave.Subscribe(parkOpenWaveModel => {
                GameManager.Instance.ParkOpenAppearManager.AppearWave(parkOpenWaveModel);
                this.parkOpenDirector.UpdateParkOpenInfo();
            }));

            // 開放が終了した時
            this.disposables.Add(this.parkOpenDirector.OnCompleted.Subscribe(_ => {
                this.OnCompleted.Execute(parkOpenGroupModel);
                this.parkOpenDirector = new NopParkOpenDirector(this.playerParkOpenRepository);
                this.parkOpenDirector.UpdateParkOpenInfo();
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