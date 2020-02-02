using System;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// パークオープン時とどうではないときの処理を分けている
    /// </summary>
    public interface IParkOpenDirector
    {
        TypeObservable<ParkOpenWaveModel> OnStartWave { get; }
        TypeObservable<int> OnCompleted { get; }
        void UpdateByFrame();
        void UpdateParkOpenInfo();
        void AddHeart(int increaseCount);
    }
    
    public class NopParkOpenDirector : IParkOpenDirector
    {
        private ParkOpenUpdateService parkOpenUpdateService = null;

        public TypeObservable<ParkOpenWaveModel> OnStartWave { get; private set; }
        public TypeObservable<int> OnCompleted { get; private set; }

        public NopParkOpenDirector(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<int>();
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
        private int currentHeartCount = 0;
        private bool isCompleted = false;
        private ParkOpenGroupModel parkOpenGroupModel;

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
        public TypeObservable<int> OnCompleted { get; private set; }

        public ParkOpenDirector(ParkOpenGroupModel parkOpenGroupModel, IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.InitializeInternal(playerParkOpenRepository);
            this.parkOpenGroupModel = parkOpenGroupModel;

            this.InitializeUpdate(); 
        }

        public ParkOpenDirector(PlayerParkOpenModel playerParkOpenModel, IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.InitializeInternal(playerParkOpenRepository);
            this.elapsedTime = playerParkOpenModel.ElapsedTime;
            this.nextWave = playerParkOpenModel.NextWave;
            this.parkOpenGroupModel = playerParkOpenModel.ParkOpenGroupModel;
            this.currentHeartCount = playerParkOpenModel.currentHeartCount;

            this.InitializeUpdate();
        }

        private void InitializeInternal(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.parkOpenObtainHeartService = new ParkOpenObtainHeartService(playerParkOpenRepository);
            this.parkOpenUpdateService = new ParkOpenUpdateService(playerParkOpenRepository);
            this.isCompleted = false;
            this.elapsedTime = 0;
            this.nextWave = 0;
            this.currentHeartCount = 0;
            this.OnStartWave = new TypeObservable<ParkOpenWaveModel>();
            this.OnCompleted = new TypeObservable<int>();
        }

        private void InitializeUpdate ()
        {
            // ハート情報の更新
            GameManager.Instance.GameUIManager.HeartPresenter.Show();
            GameManager.Instance.GameUIManager.HeartPresenter.UpdateHeart(currentHeartCount, this.parkOpenGroupModel.MaxHeartCount, this.parkOpenGroupModel.GoalHeartCount);

            // 入場者情報
            GameManager.Instance.GameUIManager.ParkOpenCharacterCountPresenter.Show();

            // デッキの表示
            var mainDeck = GameManager.Instance.ParkOpenCardManager.GetMainDeck();
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.Show();
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.SetContents(mainDeck);
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
                    this.Complete();
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

        private void Complete ()
        {
            this.OnCompleted.Execute(0);                    
            this.isCompleted = true;
            GameManager.Instance.GameUIManager.HeartPresenter.Close();
            GameManager.Instance.GameUIManager.ParkOpenCharacterCountPresenter.Close();
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.Close();
        }

        public void UpdateParkOpenInfo()
        {
            this.parkOpenUpdateService.Execute(
                true, 
                this.elapsedTime, 
                this.nextWave, 
                this.parkOpenGroupModel,
                this.currentHeartCount);
        }

        public override string ToString() {
            return string.Format("ElapsedTime:{0}\nNextAppearTime:{1}\n",elapsedTime, NextAppearTime);
        }

        public void AddHeart(int increaseCount)
        {
            this.currentHeartCount += increaseCount;
            this.parkOpenObtainHeartService.Execute(increaseCount);
            GameManager.Instance.GameUIManager.HeartPresenter.UpdateHeart(currentHeartCount, this.parkOpenGroupModel.MaxHeartCount, this.parkOpenGroupModel.GoalHeartCount);
        }        
    }    
}