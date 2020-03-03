using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ParkOpenGroupSelectMode : IGameMode {

        private List<IDisposable> disposables = new List<IDisposable>();

        public ParkOpenGroupSelectMode () {
        }

        public void OnEnter () {

            // 閉じられたら通常のステートへ
            this.disposables.Add(GameManager.Instance.GameUIManager.ParkOpenGroupsTabPresenter.OnClose
                .Subscribe(_ => {
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                }));

            // 出撃が押されたら出撃する
            this.disposables.Add(GameManager.Instance.GameUIManager.ParkOpenGroupsTabPresenter.OnStartParkOpenGroupObservable
                .SelectMany(playerParkOpenGroupModel => {
                    GameManager.Instance.GameUIManager.ParkOpenStartPresenter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenStartPresenter.SetContents(playerParkOpenGroupModel);
                    return GameManager.Instance.GameUIManager.ParkOpenStartPresenter.OnIsStartObservable
                        .Select(isStart => {
                            return isStart ? playerParkOpenGroupModel : null;
                        });
                })
                .Subscribe(playerParkOpenGroupModel => {
                    // Start する場合は開放する
                    if (playerParkOpenGroupModel != null)
                    {
                        GameManager.Instance.GameUIManager.ParkOpenGroupsTabPresenter.Close();
                        GameManager.Instance.ParkOpenManager.Open(playerParkOpenGroupModel.ParkOpenGroupModel);
                    }
                }));
        }
        public void OnUpdate () {
        }
        public void OnExit () {
            
            this.disposables.ForEach(disposable => {
                disposable.Dispose();
            });
            this.disposables.Clear();

            // セーフティー
            GameManager.Instance.GameUIManager.ParkOpenGroupsTabPresenter.Close();
        }
    }
}