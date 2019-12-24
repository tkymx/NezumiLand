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
        private MonoViewCreateService monoViewCreateService = null;
        private MonoLevelUpServive monoLevelUpServive = null;
        private MonoViewRemoveService monoViewRemoveService = null;

        public MonoManager (GameObject root, IPlayerMonoViewRepository playerMonoViewRepository) {
            this.root = root;
            this.monoViewModels = new List<MonoViewModel> ();
            this.monoViewCreateService = new MonoViewCreateService(playerMonoViewRepository);
            this.monoLevelUpServive = new MonoLevelUpServive(playerMonoViewRepository);
            this.monoViewRemoveService = new MonoViewRemoveService(playerMonoViewRepository);
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

            // プレイヤー情報の作成
            var playerMonoViewModel = this.monoViewCreateService.Execute(monoInfo.Id);

            var monoViewModel = new MonoViewModel (monoView, playerMonoViewModel);
            monoViewModels.Add (monoViewModel);

            return monoViewModel;
        }

        public MonoViewModel CreateMono(PlayerMonoViewModel playerMonoViewModel, Vector3 position) {
            var instance = Object.AppearToFloor (playerMonoViewModel.MonoInfo.MonoPrefab, root, position);
            var monoView = instance.GetComponent<MonoView> ();
            Debug.Assert (monoView != null, "monoViewではありません");

            var monoViewModel = new MonoViewModel (monoView, playerMonoViewModel);
            monoViewModels.Add (monoViewModel);

            return monoViewModel;
        }

        public void LevelUp(PlayerMonoViewModel playerMonoViewModel) {
            this.monoLevelUpServive.Execute(playerMonoViewModel);
        }

        public void RemoveMono (MonoViewModel monoViewModel) {
            Debug.Assert (this.monoViewModels.Contains (monoViewModel), "消去予定のものが存在しません");
            Object.DisAppear (monoViewModel.MonoView.gameObject);
            monoViewModels.Remove (monoViewModel);

            // 消去
            this.monoViewRemoveService.Execute(monoViewModel);
        }
    }
}