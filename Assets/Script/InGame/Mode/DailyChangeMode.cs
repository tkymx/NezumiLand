using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// 日々の境目を担当
    /// </summary>
    public class DailyChangeMode : IGameMode {

        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;
        private readonly DailyEarnCalculater dailyEarnCalculater = null;
        private readonly SatisfactionCalculater satisfactionCalculater = null;

        public DailyChangeMode (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.dailyEarnCalculater = new DailyEarnCalculater(playerOnegaiRepository);
            this.satisfactionCalculater = new SatisfactionCalculater(playerOnegaiRepository);
        }
        
        public void OnEnter () {
            GameManager.Instance.TimeManager.Pause ();

            // 変化前の情報
            Currency prevCurrency = GameManager.Instance.Wallet.CurrentCurrency;
            Satisfaction currentSatisfaction = this.satisfactionCalculater.CalcFieldSatisfaction();

            // 日銭の計算と付与を行う
            var dailyEarn = dailyEarnCalculater.CalcEarnFromSatisfaction ();
            GameManager.Instance.Wallet.Push (dailyEarn);
            GameManager.Instance.EffectManager.PlayEarnEffect (dailyEarn, GameManager.Instance.MouseHomeManager.HomePostion);

            // 変化後の計算
            Currency nextCurrency = GameManager.Instance.Wallet.CurrentCurrency;
            
            // 一日の終りダイアログを出す
            this.DoDailyEnd ();
            GameManager.Instance.GameUIManager.DailyEndPresenter.Show ();
            GameManager.Instance.GameUIManager.DailyEndPresenter.SetCurrentChangeInfo(prevCurrency, nextCurrency, currentSatisfaction);
            GameManager.Instance.GameUIManager.DailyEndPresenter.OnClose
                .SelectMany(_ => {
                    // 一日の始まりダイアログを出す
                    this.DoDailyStart ();
                    GameManager.Instance.GameUIManager.DailyStartPresenter.Show();
                    return GameManager.Instance.GameUIManager.DailyStartPresenter.OnClose;
                })
                .Subscribe(_ => {
                    // 選択画面に移動する
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                });
        }
        public void OnUpdate () {
            if (GameManager.Instance.GameUIManager.DailyEndPresenter.IsShow ()) {
                return;
            }
            if (GameManager.Instance.GameUIManager.DailyStartPresenter.IsShow ()) {
                return;
            }
            // どちらも表示していなかったら、選択画面に移動する
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
        }
        public void OnExit () {
            GameManager.Instance.TimeManager.Play ();
        }

        /// <summary>
        /// 一日の終りに行うこと
        /// </summary>
        private void DoDailyEnd() {
            this.RemoveAppearCharacter();
        }

        private void RemoveAppearCharacter() {
            GameManager.Instance.AppearCharacterManager.RemoveAll();
        }

        /// <summary>
        /// 一日の始まりに行うこと
        /// </summary>
        private void DoDailyStart() {
            this.ResistAppearCharacter();
        }

        private void ResistAppearCharacter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.Resist();
        }        
    }
}