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
            this.disposables.Add(GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.OnClose
                .Subscribe(_ => {
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                }));

            // 出撃が押されたら出撃する
            this.disposables.Add(GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.OnStartParkOpenGroupObservable
                .SelectMany(parkOpenGroupModel => {
                    GameManager.Instance.GameUIManager.ParkOpenStartPresenter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenStartPresenter.SetContents(parkOpenGroupModel);
                    return GameManager.Instance.GameUIManager.ParkOpenStartPresenter.OnIsStartObservable
                        .Select(isStart => {
                            return isStart ? parkOpenGroupModel : null;
                        });
                })
                .Subscribe(parkOpenGroupModel => {
                    // Start する場合は開放するが、しない場合はそのまま
                    if (parkOpenGroupModel != null)
                    {
                        GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.Close();
                        GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.Close();
                        GameManager.Instance.ParkOpenManager.Open(parkOpenGroupModel);
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
            GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.Close();
        }
    }
}