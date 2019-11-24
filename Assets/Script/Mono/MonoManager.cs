using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 存在しているMonoの存在を管理するクラス
    /// </summary>
    public class MonoManager {
        private GameObject root;
        private List<MonoViewModel> monoViewModels;

        public MonoManager (GameObject root) {
            this.root = root;
            this.monoViewModels = new List<MonoViewModel> ();
        }

        public void UpdateByFrame () {
            foreach (var monoViewModel in this.monoViewModels) {
                monoViewModel.UpdateByFrame ();
            }
        }

        public Satisfaction GetAllSatisfaction () {
            Satisfaction satisfaction = new Satisfaction (0);
            foreach (var monoViewModel in monoViewModels) {
                satisfaction += monoViewModel.GetCurrentSatisfaction ();
            }
            return satisfaction;
        }

        public MonoViewModel CreateMono (MonoInfo monoInfo, Vector3 position) {
            var instance = Object.AppearToFloor (monoInfo.MonoPrefab, root, position);
            var monoView = instance.GetComponent<MonoView> ();
            Debug.Assert (monoView != null, "monoViewではありません");

            var monoViewModel = new MonoViewModel (monoView, monoInfo);
            monoViewModels.Add (monoViewModel);

            return monoViewModel;
        }

        public void RemoveMono (MonoViewModel monoViewModel) {
            Debug.Assert (this.monoViewModels.Contains (monoViewModel), "消去予定のものが存在しません");
            Object.DisAppear (monoViewModel.MonoView.gameObject);
            monoViewModels.Remove (monoViewModel);
        }
    }
}