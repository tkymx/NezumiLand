﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {

    /// <summary>
    /// インスタンス化されたキャラクタの行動を管理
    /// </summary>
    public class AppearCharacterViewModel
    {
        public PlayerAppearCharacterViewModel PlayerAppearCharacterViewModel { get; private set; }
        private AppearCharacterView appearCharacterView;
        private IAppearCharacterLifeDirector appearCharacterLifeDirector;

        public Vector3 Position => this.appearCharacterView.transform.position;

        private List<IDisposable> disposables = new List<IDisposable>();

        private StateManager stateManager;

        // 消失する地点
        public Vector3 DisappearPosition => PlayerAppearCharacterViewModel.MovePath.DisapearPosition;

        public bool IsParkOpenCharacter => PlayerAppearCharacterViewModel.AppearCharacterLifeDirectorType == AppearCharacterLifeDirectorType.ParkOpen;

        public void InterruptState(AppearCharacterState appearCharacterState) 
        {
            if (appearCharacterState == AppearCharacterState.GoMono) {
                stateManager.Interrupt (new GoMonoState (this));
            }
            else if (appearCharacterState == AppearCharacterState.GoAway) {
                stateManager.Interrupt (new GoAwayState (this));
            }
            else if (appearCharacterState == AppearCharacterState.PlayingMono) {
                stateManager.Interrupt (new PlayingMonoState (this));
            }
            else if (appearCharacterState == AppearCharacterState.Removed) {
                stateManager.Interrupt (new RemovedState ());
            }
            else if (appearCharacterState == AppearCharacterState.None) {
                stateManager.Interrupt (new EmptyState ());
            }
            else {
                Debug.Assert(false, "ありえない状態" + appearCharacterState.ToString());
            }
        }

        private IAppearCharacterLifeDirector CreateDirector(PlayerAppearCharacterViewModel playerAppearCharacterViewModel)
        {
            if (playerAppearCharacterViewModel.AppearCharacterLifeDirectorType == AppearCharacterLifeDirectorType.ParkOpen) 
            {
                var directerModel = playerAppearCharacterViewModel.PlayerAppearCharacterDirectorModelBase as PlayerAppearParkOpenCharacterDirectorModel;
                Debug.Assert(directerModel != null, "AppearConversationCharacterDirectorModelではありません");
                return new ParkOpenAppearCharacterLifeDirector(this, directerModel);
            } 
            else if (playerAppearCharacterViewModel.AppearCharacterLifeDirectorType == AppearCharacterLifeDirectorType.Conversation) 
            {
                var directerModel = playerAppearCharacterViewModel.PlayerAppearCharacterDirectorModelBase as PlayerAppearConversationCharacterDirectorModel;
                Debug.Assert(directerModel != null, "AppearConversationCharacterDirectorModelではありません");
                return new ReserveAppearCharacterLifeDirector(directerModel);
            } 
            else 
            {
                Debug.Assert(false, "ありえない状態" + playerAppearCharacterViewModel.AppearCharacterLifeDirectorType.ToString());
            }
            return null;
        }

        public AppearCharacterViewModel(AppearCharacterView appearCharacterView, PlayerAppearCharacterViewModel playerAppearCharacterViewModel)
        {
            this.appearCharacterView = appearCharacterView;            
            this.PlayerAppearCharacterViewModel = playerAppearCharacterViewModel;

            this.stateManager = new StateManager(new EmptyState());
            this.disposables.Add(this.stateManager.OnChangeStateObservable.Subscribe(state => {
                GameManager.Instance.AppearCharacterManager.ChangeState(playerAppearCharacterViewModel, state);
            }));

            // Director の作成
            this.appearCharacterLifeDirector = CreateDirector(playerAppearCharacterViewModel);

            // 遊んでいた時間を適応
            this.playeringTime = playerAppearCharacterViewModel.CurrentPlayingTime;

            // キャラクタがタップされた時
            disposables.Add(appearCharacterView.OnSelectObservable
                .SelectMany(_ => {
                    return this.appearCharacterLifeDirector.OnTouch();
                })
                .Subscribe(_ => {
                }));

            this.appearCharacterLifeDirector.OnInitializeView(appearCharacterView);

            // 生成
            this.appearCharacterLifeDirector.OnCreate();
        }

        public void UpdateByFrame()
        {
            this.InitializeByFrame();
            this.stateManager.UpdateByFrame();
            this.Move();
            this.UpdateTransform();
        }

        private float playeringTime = 0;

        public void StartPlaying () {
            this.playeringTime = 0;
        }

        public bool IsPlayingFinish () {
            return this.playeringTime > 2.0f;
        }

        public void UpdatePlaying () {
            if (this.stateManager.CurrentState is PlayingMonoState) {
                this.playeringTime += GameManager.Instance.TimeManager.DeltaTime ();
            }
        }

        // 移動関連
        
        private Vector3 moveVector;

        void InitializeByFrame () {
            moveVector = Vector3.zero;
        }

        void Move () {
            if (moveVector.magnitude > 0.001) {
                this.appearCharacterView.ChangeAnimation("run");
                this.appearCharacterView.SetRotation(Quaternion.LookRotation (moveVector));
                this.appearCharacterView.SetPosition(this.appearCharacterView.transform.position + moveVector);
            } else {
                this.appearCharacterView.ChangeAnimation("idle");
            }
        }

        public void MoveTo (Vector3 target)
        {
            moveVector = ObjectComparison.Direction (target, this.appearCharacterView.transform.position) * 4.0f * GameManager.Instance.TimeManager.DeltaTime ();
        }

        private float elapsedTime = 0;
        private const float intervalTime = 1.0f;
        public void UpdateTransform()
        {
            elapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (elapsedTime > intervalTime) {
                this.elapsedTime = 0;

                // 座標の更新
                GameManager.Instance.AppearCharacterManager.ChangeTransform(
                    this.PlayerAppearCharacterViewModel,
                    this.appearCharacterView.transform.position,
                    this.appearCharacterView.transform.rotation.eulerAngles);

                // 遊んでいる時間を適応
                if (this.stateManager.CurrentState is PlayingMonoState) {
                    GameManager.Instance.AppearCharacterManager.SetCurrentPlayingTime(
                        this.PlayerAppearCharacterViewModel,
                        this.playeringTime
                    );
                }
            }
        }

        public void Dispose () 
        {
            this.appearCharacterLifeDirector.OnRemove();

            Object.DisAppear(appearCharacterView.gameObject);

            // dispose を駆逐
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();
        }
    }
}