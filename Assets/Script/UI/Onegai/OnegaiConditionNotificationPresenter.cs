using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum OnegaiConditionNotificationState {
        None,
        Clear,
        Failue        
    }

    public class OnegaiConditionNotificationPresenter : UiWindowPresenterBase
    {
        private class OnegaiConditionNotificationEntry {
            public OnegaiModel OnegaiModel { get; private set; }
            public OnegaiConditionNotificationState State { get; private set; }

            public OnegaiConditionNotificationEntry(OnegaiModel OnegaiModel, OnegaiConditionNotificationState State) {
                this.OnegaiModel = OnegaiModel;
                this.State = State;
            }
        }        

        private static float ShowingTime = 4;

        [SerializeField]
        private OnegaiConditionNotificationView onegaiConditionNotificationView = null;

        private Queue<OnegaiConditionNotificationEntry> entryQueue = null;

        private float elapsedShowTime = 0;

        public void Initialize() {
            this.entryQueue = new Queue<OnegaiConditionNotificationEntry>();
            this.elapsedShowTime = 0;

            this.onegaiConditionNotificationView.Initialize();
            this.disposables.Add(onegaiConditionNotificationView.OnCloseObservable.Subscribe(_ => {
                this.CloseInternal();
            }));
            this.Close();
        }

        public void CloseInternal () {
            this.Close();
            if (this.entryQueue.Count > 0) {
                this.ShowByEntry(this.entryQueue.Dequeue());
            }
        }

        public void UpdateByFrame () {
            if (this.IsShow()) {            
                // 時間終了
                this.elapsedShowTime += GameManager.Instance.TimeManager.DeltaTimeWithoutPause ();
                if (this.elapsedShowTime > OnegaiConditionNotificationPresenter.ShowingTime) {
                    this.CloseInternal();
                }
            }
        }
        
        public override void onPrepareShow() {
            this.elapsedShowTime = 0;
        }

        private void SetContents(OnegaiConditionNotificationEntry entry) {
            this.onegaiConditionNotificationView.UpdateView(entry.OnegaiModel.Title, "満足度 : " + entry.OnegaiModel.Satisfaction.ToString(), entry.State);
        }

        public void PushNotification (OnegaiModel onegaiModel, OnegaiConditionNotificationState state) {
            var entry = new OnegaiConditionNotificationEntry(onegaiModel, state);
            if(this.IsShow()) {
                this.entryQueue.Enqueue(entry);
            } else {
                this.ShowByEntry(entry);
            }
        }

        private void ShowByEntry (OnegaiConditionNotificationEntry entry) {
            this.SetContents(entry);
            this.Show();
            this.elapsedShowTime = 0;
        }
    }    
}