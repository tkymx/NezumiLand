using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenManager : IDisposable {

        private IParkOpenDirector parkOpenDirector = null;
        private List<IDisposable> disposables = null;
        public TypeObservable<ParkOpenGroupModel> OnCompleted { get; private set; }

        private ParkOpenUpdateService parkOpenUpdateService = null;

        public ParkOpenManager(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.disposables = new List<IDisposable>();
            this.parkOpenDirector = new NopParkOpenDirector();
            this.OnCompleted = new TypeObservable<ParkOpenGroupModel>();

            this.parkOpenUpdateService = new ParkOpenUpdateService(playerParkOpenRepository);
        }

        public void Open(ParkOpenGroupModel parkOpenGroupModel)
        {
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");
            this.parkOpenDirector = new ParkOpenDirector(parkOpenGroupModel);
            this.SetEventInternal(parkOpenGroupModel);
            this.parkOpenDirector.UpdateParkOpenInfo(this.parkOpenUpdateService);
        }

        public void Open(PlayerParkOpenModel playerParkOpenModel)
        {
            Debug.Assert(this.parkOpenDirector is NopParkOpenDirector, "すでに実行中のDirectorが存在します。");
            this.parkOpenDirector = new ParkOpenDirector(playerParkOpenModel);
            this.SetEventInternal(playerParkOpenModel.ParkOpenGroupModel);
            this.parkOpenDirector.UpdateParkOpenInfo(this.parkOpenUpdateService);
        }

        private void SetEventInternal(ParkOpenGroupModel parkOpenGroupModel)
        {
            // イベントのセット
            this.ClearDisposables();
            this.disposables.Add(this.parkOpenDirector.OnStartWave.Subscribe(parkOpenWaveModel => {
                GameManager.Instance.ParkOpenAppearManager.AppearWave(parkOpenWaveModel);
                this.parkOpenDirector.UpdateParkOpenInfo(this.parkOpenUpdateService);
            }));
            this.disposables.Add(this.parkOpenDirector.OnCompleted.Subscribe(_ => {
                this.OnCompleted.Execute(parkOpenGroupModel);
                this.parkOpenDirector = new NopParkOpenDirector();
                this.parkOpenDirector.UpdateParkOpenInfo(this.parkOpenUpdateService);
            }));
        }

        private float elapsedTime = 0;

        public void UpdateByFrame()
        {
            // 一定時間ごとに開放の状況を記録する
            this.elapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (elapsedTime > 1.0f) {
                this.parkOpenDirector.UpdateParkOpenInfo(this.parkOpenUpdateService);
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
        }

        public override string ToString()
        {
            return this.parkOpenDirector.ToString();
        }
    }

    public interface IParkOpenDirector
    {
        TypeObservable<ParkOpenWaveModel> OnStartWave { get; }
        TypeObservable<int> OnCompleted { get; }
        void UpdateByFrame();
        void UpdateParkOpenInfo(ParkOpenUpdateService parkOpenUpdateService);
    }
    
    public class NopParkOpenDirector : IParkOpenDirector
    {
        public TypeObservable<ParkOpenWaveModel> OnStartWave { get; private set; }
        public TypeObservable<int> OnCompleted { get; private set; }

        public NopParkOpenDirector()
        {
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<int>();
        }

        public void UpdateByFrame()
        {

        }

        public void UpdateParkOpenInfo(ParkOpenUpdateService parkOpenUpdateService)
        {
            parkOpenUpdateService.Execute(false);
        }
    }

    /// <summary>
    /// ParkOpenDirector
    /// 動物園開放時の流れの指揮とりを行う
    /// インスタンス化されてすぐに実行され、あとは毎フレームの更新を待つ
    /// </summary>
    public class ParkOpenDirector : IParkOpenDirector
    {
        private readonly float OpenTime = 20;   //TODO: 20秒でいいのか、もっと増やすのかは相談

        private int nextWave;
        private float elapsedTime = 0;
        private bool isCompleted = false;

        private ParkOpenGroupModel parkOpenGroupModel;

        /// <summary>
        /// ウェーブが始まった時
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenWaveModel> OnStartWave { get; private set; }

        /// <summary>
        /// 公園開放が終わった時
        /// </summary>
        /// <value></value>
        public TypeObservable<int> OnCompleted { get; private set; }

        public ParkOpenDirector(ParkOpenGroupModel parkOpenGroupModel)
        {
            this.InitializeInternal();
            this.parkOpenGroupModel = parkOpenGroupModel;
        }

        public ParkOpenDirector(PlayerParkOpenModel playerParkOpenModel)
        {
            this.InitializeInternal();
            this.elapsedTime = playerParkOpenModel.ElapsedTime;
            this.nextWave = playerParkOpenModel.NextWave;
            this.parkOpenGroupModel = playerParkOpenModel.ParkOpenGroupModel;
        }

        private void InitializeInternal()
        {
            this.isCompleted = false;
            this.elapsedTime = 0;
            this.nextWave = 0;
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<int>();            
        }

        /// <summary>
        /// 一つの Wave の間隔の時間
        /// 発生位置が中間である必要があるため、＋１
        /// </summary>
        private float WaveInterval => OpenTime / (this.parkOpenGroupModel.ParkOpenWaves.Length + 1); 

        /// <summary>
        /// 次の出現までの時間
        /// </summary>
        private float NextAppearTime => WaveInterval * (nextWave + 1);

        /// <summary>
        /// 最終Wave かどうか？
        /// </summary>
        private bool IsFinalWave => this.nextWave >= parkOpenGroupModel.ParkOpenWaves.Length;

        public void UpdateByFrame()
        {
            if (this.isCompleted) {
                return;
            }

            this.elapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (this.elapsedTime > NextAppearTime)
            {
                // 最後だったら終了
                if (this.IsFinalWave) {
                    this.OnCompleted.Execute(0);
                    this.isCompleted = true;
                }
                else
                {
                    // wave開始
                    this.OnStartWave.Execute(parkOpenGroupModel.ParkOpenWaves[this.nextWave]);
                    // 次の Wave にする
                    this.nextWave++;             
                }
            }
        }

        public void UpdateParkOpenInfo(ParkOpenUpdateService parkOpenUpdateService)
        {
            parkOpenUpdateService.Execute(
                true, 
                this.elapsedTime, 
                this.nextWave, 
                this.parkOpenGroupModel);
        }


        public override string ToString() {
            return string.Format("ElapsedTime:{0}\nNextAppearTime:{1}\n",elapsedTime, NextAppearTime);
        }
    }    
}