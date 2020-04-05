using System.Collections.Generic;
using System;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// パークオープン時とどうではないときの処理を分けている
    /// </summary>
    public interface IParkOpenDirector : IDisposable
    {
        TypeObservable<ParkOpenWaveModel> OnStartWave { get; }
        TypeObservable<ParkOpenResultAmount> OnCompleted { get; }
        ParkOpenTimeAmount CurrentParkOpenTimeAmount { get; }
        void UpdateByFrame();
        void UpdateParkOpenInfo();
        void AddHeart(int increaseCount);
    }
    
    public class NopParkOpenDirector : IParkOpenDirector
    {
        private ParkOpenUpdateService parkOpenUpdateService = null;

        public TypeObservable<ParkOpenWaveModel> OnStartWave { get; private set; }
        public TypeObservable<ParkOpenResultAmount> OnCompleted { get; private set; }

        public ParkOpenTimeAmount CurrentParkOpenTimeAmount => new ParkOpenTimeAmount(0,ParkOpenWaveCounter.OpenTime);

        public NopParkOpenDirector(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<ParkOpenResultAmount>();
            this.parkOpenUpdateService = new ParkOpenUpdateService(playerParkOpenRepository);
        }

        public void UpdateByFrame()
        {

        }

        public void UpdateParkOpenInfo()
        {
            parkOpenUpdateService.Execute(false);
        }

        public void AddHeart(int increaseCount)
        {

        }

        public void Dispose()
        {

        }
    }

    class ParkOpenWaveCounter
    {
        /// <summary>
        /// 開放の時間
        /// 20秒でいいのか、もっと増やすのかは相談
        /// </summary>
        public static readonly float OpenTime = 20;


        /// <summary>
        /// ウェーブのカウント
        /// </summary>
        private int waveCount;

        /// <summary>
        /// 経過時間
        /// </summary>
        /// <value></value>
        public float ElapsedTime { get; private set; }

        /// <summary>
        /// 次のウェーブ
        /// </summary>
        /// <value></value>
        public int NextWave { get; private set; }

        /// <summary>
        /// 一つの Wave の間隔の時間
        /// 発生位置が中間である必要があるため、＋１
        /// </summary>
        private float WaveInterval => ParkOpenWaveCounter.OpenTime / (waveCount + 1); 

        /// <summary>
        /// 次の出現までの時間
        /// </summary>
        public float NextAppearTime => WaveInterval * (NextWave + 1);

        /// <summary>
        /// 時間情報
        /// </summary>
        /// <returns></returns>
        public ParkOpenTimeAmount CurrentParkOpenTimeAmount => new ParkOpenTimeAmount(this.ElapsedTime,ParkOpenWaveCounter.OpenTime);

        /// <summary>
        /// 最終Wave かどうか？
        /// </summary>
        private bool IsFinalWave => this.NextWave >= waveCount;

        private bool isCompleted = false;

        public TypeObservable<int> OnCompletedObservable { get; private set; }

        public TypeObservable<int> OnStartWaveObservable { get; private set; }

        public ParkOpenWaveCounter(int waveCount, float elapsedTime = 0, int nextWave = 0)
        {
            this.OnCompletedObservable = new TypeObservable<int>();
            this.OnStartWaveObservable = new TypeObservable<int>();
            this.waveCount = waveCount;
            this.ElapsedTime = elapsedTime;
            this.NextWave = nextWave;
            this.isCompleted = false;
        }

        public void UpdateByFrame()
        {
            if (this.isCompleted) {
                return;
            }

            this.ElapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (this.ElapsedTime > NextAppearTime)
            {
                // 最後だったら終了
                if (this.IsFinalWave) {
                    this.isCompleted = true;
                    this.OnCompletedObservable.Execute(0);
                }
                else
                {
                    // wave開始
                    this.OnStartWaveObservable.Execute(this.NextWave);
                    // 次の Wave にする
                    this.NextWave++;             
                }
            }
        }

        public override string ToString() {
            return string.Format("ElapsedTime:{0}\nNextAppearTime:{1}\n", ElapsedTime, NextAppearTime);
        }        

    }    

    /// <summary>
    /// ParkOpenDirector
    /// 動物園開放時の流れの指揮とりを行う
    /// インスタンス化されてすぐに実行され、あとは毎フレームの更新を待つ
    /// </summary>
    public class ParkOpenDirector : IParkOpenDirector
    {
        private int currentHeartCount = 0;

        private PlayerParkOpenGroupModel playerParkOpenGroupModel;
        private ParkOpenGroupModel parkOpenGroupModel => playerParkOpenGroupModel.ParkOpenGroupModel;

        private ParkOpenWaveCounter waveCounter;

        private ParkOpenObtainHeartService parkOpenObtainHeartService = null;
        private ParkOpenUpdateService parkOpenUpdateService = null;

        /// <summary>
        /// ウェーブが始まった時
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenWaveModel> OnStartWave { get; private set; }

        /// <summary>
        /// 公園開放が終わった時
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenResultAmount> OnCompleted { get; private set; }

        /// <summary>
        /// 時間情報
        /// </summary>
        /// <returns></returns>
        public ParkOpenTimeAmount CurrentParkOpenTimeAmount => this.waveCounter.CurrentParkOpenTimeAmount;

        private List<IDisposable> disposables = new List<IDisposable>();

        public ParkOpenDirector(PlayerParkOpenGroupModel playerParkOpenGroupModel, IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenGroupModel = playerParkOpenGroupModel;
            this.currentHeartCount = 0;

            this.InitializeInternal(playerParkOpenRepository);
            this.InitializeWave(playerParkOpenGroupModel.ParkOpenGroupModel.ParkOpenWaves.Length);

            this.PrepareParkOpen(); 
        }

        public ParkOpenDirector(PlayerParkOpenModel playerParkOpenModel, IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenGroupModel = playerParkOpenModel.PlayerParkOpenGroupModel;
            this.currentHeartCount = playerParkOpenModel.currentHeartCount;

            this.InitializeInternal(playerParkOpenRepository);
            this.InitializeWave(parkOpenGroupModel.ParkOpenWaves.Length, playerParkOpenModel.ElapsedTime, playerParkOpenModel.NextWave);

            this.PrepareParkOpen();
        }

        private void InitializeInternal(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.parkOpenObtainHeartService = new ParkOpenObtainHeartService(playerParkOpenRepository);
            this.parkOpenUpdateService = new ParkOpenUpdateService(playerParkOpenRepository);
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<ParkOpenResultAmount>();
        }

        private void InitializeWave(int waveCount, float elapsedTime = 0, int nextWave = 0)
        {
            this.waveCounter = new ParkOpenWaveCounter(waveCount, elapsedTime, nextWave);
            
            this.disposables.Add(this.waveCounter.OnStartWaveObservable.Subscribe(waveIndex => {
                this.OnStartWave.Execute(this.parkOpenGroupModel.ParkOpenWaves[this.waveCounter.NextWave]);
            }));

            this.disposables.Add(this.waveCounter.OnCompletedObservable.Subscribe(_ => {
                this.Complete();
            }));
        }

        /// <summary>
        /// UIの設定
        /// </summary>
        private void PrepareParkOpen ()
        {
            // ハート情報の更新
            GameManager.Instance.GameUIManager.HeartPresenter.Show();
            GameManager.Instance.GameUIManager.HeartPresenter.UpdateHeart(currentHeartCount, this.parkOpenGroupModel.MaxHeartCount, this.parkOpenGroupModel.GoalHeartCount);

            // 入場者情報
            GameManager.Instance.GameUIManager.ParkOpenCharacterCountPresenter.Show();

            // 時間
            GameManager.Instance.GameUIManager.ParkOpenTimePresenter.Show();
        }

        private void Complete ()
        {
            // ハート
            GameManager.Instance.GameUIManager.HeartPresenter.Close();

            // 人数
            GameManager.Instance.GameUIManager.ParkOpenCharacterCountPresenter.Close();

            // 時間
            GameManager.Instance.GameUIManager.ParkOpenTimePresenter.Close();

            this.OnCompleted.Execute(new ParkOpenResultAmount(this.playerParkOpenGroupModel, currentHeartCount, this.parkOpenGroupModel.GoalHeartCount, this.parkOpenGroupModel.ObtainedHeartRewards));                    
        }

        public void UpdateByFrame()
        {
            this.waveCounter.UpdateByFrame();
        }

        public void UpdateParkOpenInfo()
        {
            this.parkOpenUpdateService.Execute(
                true, 
                this.waveCounter.ElapsedTime, 
                this.waveCounter.NextWave, 
                this.playerParkOpenGroupModel,
                this.currentHeartCount);
        }

        public void AddHeart(int increaseCount)
        {
            this.currentHeartCount += increaseCount;
            GameManager.Instance.GameUIManager.HeartPresenter.UpdateHeart(currentHeartCount, this.parkOpenGroupModel.MaxHeartCount, this.parkOpenGroupModel.GoalHeartCount);
            this.parkOpenObtainHeartService.Execute(increaseCount);
        }

        public void Dispose()
        {
            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
            this.disposables.Clear();
        }

        public override string ToString() {
            return this.waveCounter.ToString();
        }        
    }    
}