using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {
    /// <summary>
    /// 存在しているMonoの存在を管理するクラス
    /// </summary>
    public class MonoManager {
        private GameObject root;
        private Camera camera;

        private List<MonoViewModel> monoViewModels;
        private MonoViewCreateService monoViewCreateService = null;
        private MonoLevelUpServive monoLevelUpServive = null;
        private MonoViewRemoveService monoViewRemoveService = null;

        public MonoManager (Camera camera, GameObject root, IPlayerMonoViewRepository playerMonoViewRepository) {
            this.camera = camera;
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
            monoViewModel.Dispose();
            monoViewModels.Remove (monoViewModel);

            // 消去
            this.monoViewRemoveService.Execute(monoViewModel);
        }

        /// <summary>
        /// 宣伝を行うための実行者を取得
        /// </summary>
        /// <returns></returns>
        public MonoPromotionCreater GenerateMonoPromotionCreater()
        {
            return new MonoPromotionCreater(this.monoViewModels,this.camera, this.root);
        }
    }

    public class MonoPromotionCreater
    {
        private List<MonoViewModel> monoViewModels;
        private Camera camera;
        private GameObject root;

        /// <summary>
        /// ものの宣伝をタップした。
        /// </summary>
        /// <value></value>
        public TypeObservable<MonoViewModel> OnTouchPromotion { get; private set; }

        /// <summary>
        /// もののすべての宣伝をタップした。
        /// </summary>
        /// <value></value>
        public TypeObservable<int> OnAllTouchPromotion { get; private set; }

        private List<IDisposable> disposables;

        public MonoPromotionCreater(List<MonoViewModel> monoViewModels,Camera camera, GameObject root)
        {
            this.monoViewModels = monoViewModels;
            this.camera = camera;
            this.root = root;

            this.OnTouchPromotion = new TypeObservable<MonoViewModel>();
            this.OnAllTouchPromotion = new TypeObservable<int>();
            this.disposables = new List<IDisposable>();
        }

        public void ShowPromotion()
        {
            // disposable をすべて消す
            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
            this.disposables.Clear();

            // 宣伝を設置する
            foreach (var monoViewModel in this.monoViewModels)
            {
                this.disposables.Add(monoViewModel.ShowPromotion(camera, root).Subscribe(_ => {
                    this.OnTouchPromotion.Execute(monoViewModel);

                    // 宣伝が一つもなければすべてタッチしたとみなす。
                    if (!this.IsShowPromotion())
                    {
                        this.OnAllTouchPromotion.Execute(0);
                    }
                }));
            }
        }

        /// <summary>
        /// 宣伝を持っているか？
        /// </summary>
        /// <returns></returns>
        private bool IsShowPromotion()
        {
            return this.monoViewModels.Any(monoViewModel => monoViewModel.HasPromotion);
        }
    }
}