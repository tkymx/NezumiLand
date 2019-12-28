using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class PreMono {
        private Mouse mouse;
        private GameObject makingPrefab;
        private MonoInfo mono;
        private GameObject makingInstane;
        private PreMonoView preMonoView = null;
        
        private MakingAmount currentMakingAmount;
        public MakingAmount CurrentMakingAmount => currentMakingAmount;

        public PreMono (Mouse mouse, GameObject makingPrefab, MonoInfo mono) {
            this.mouse = mouse;
            this.makingPrefab = makingPrefab;
            this.mono = mono;
        }

        public void StartMaking (PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.makingInstane = Object.AppearToFloor (makingPrefab, mouse.transform.parent.gameObject, playerArrangementTargetModel.CenterPosition);
            
            this.preMonoView = this.makingInstane.GetComponent<PreMonoView>();
            Debug.Assert(this.preMonoView != null, "PreMonoView がセットされていません");

            this.currentMakingAmount = new MakingAmount(0,mono.MakingTime);
            this.preMonoView.UpdateView(this.currentMakingAmount.Rate);
        }

        public void ProgressMaking (MakingAmount deltaMakingAmount) {
            this.currentMakingAmount += deltaMakingAmount;
            this.preMonoView.UpdateView(this.currentMakingAmount.Rate);
        }

        public bool IsFinishMaking () {
            return this.currentMakingAmount.IsFinish;
        }

        public void FinishMaking (PlayerArrangementTargetModel playerArrangementTargetModel) {
            Debug.Assert (playerArrangementTargetModel.PlayerMonoViewModel == null, "モノがセットされています。");
            Object.DisAppear (this.makingInstane);

            // ものを生成
            GameManager.Instance.ArrangementManager.CreateAndSetMono(playerArrangementTargetModel);
        }
    }
}