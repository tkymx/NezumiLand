using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace NL.EventContents {
    public class ScreenImages : EventContentsBase {
        private Boolean isAlive = true;
        private string[] imageNames = null;

        public ScreenImages(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            this.imageNames = playerEventModel.EventModel.EventContentsModel.Arg[0].Split(',');
            this.isAlive = true;
        }

        public override EventContentsType EventContentsType => EventContentsType.ScreenImages;

        public override void OnEnter() {
            this.isAlive = true;
            ShowImage();
        }

        private IDisposable disposable = null;

        private void ShowImage (int index = 0) {
            if (index >= this.imageNames.Length) {
                this.isAlive = false;
                return;
            } 

            var sprite = ResourceLoader.LoadScreenSprite(this.imageNames[index]);
            GameManager.Instance.GameUIManager.ImagePresenter.SetImage(sprite, "チュートリアル");
            GameManager.Instance.GameUIManager.ImagePresenter.Show();
            this.disposable = GameManager.Instance.GameUIManager.ImagePresenter.OnClose.Subscribe(_ => {
                if (this.disposable != null) {
                    this.disposable.Dispose();                    
                }
                this.ShowImage(index+1);
            });
        }

        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
        public override bool IsAlive() {
            return isAlive;
        }

        public override string ToString() {
            return this.EventContentsType.ToString();
        }
    }
}