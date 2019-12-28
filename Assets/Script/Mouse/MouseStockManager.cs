using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MouseStockManager : ConsumableCollectionBase<MouseOrderAmount> {
        private readonly IPlayerMouseStockRepository playerMouseStockRepository;
        private readonly IPlayerMouseViewRepository playerMouseViewRepository;

        private MouseCreateService mouseCreateService;
        private MouseRemoveService mouseRemoveService;
        private MouseChangeTransformService mouseChangeTransformService;
        private MouseChangeStateService mouseChangeStateService;
        private MouseChangeMakingAmountService mouseChangeMakingAmountService;

        private GameObject root;
        private List<Mouse> orderedMouse;

        /// <summary>
        /// ネズミのストック数
        /// いずれ増やせるようになりたい
        /// </summary>
        private MouseOrderAmount mouseStockCount;
        public MouseOrderAmount MouseStockCount => mouseStockCount;

        /// <summary>
        /// ネズミをオーダーしている数
        /// </summary>
        public override MouseOrderAmount Current => new MouseOrderAmount(orderedMouse.Count);
        public override MouseOrderAmount CurrentWithReserve => Current + GameManager.Instance.ReserveAmountManager.Get<MouseOrderAmount>();

        /// <summary>
        /// ネズミにオーダーできるか？
        /// </summary>
        public override bool OnIsConsume (MouseOrderAmount count) {
            return (this.Current + count) <= this.mouseStockCount;
        }

        public override void OnConsume(MouseOrderAmount amount) {
            Debug.Assert(false,"OrderMouseでオーダーしているので基本的には通らない");
            throw new System.NotImplementedException();
        }

        public MouseStockManager (GameObject root, IPlayerMouseStockRepository playerMouseStockRepository, IPlayerMouseViewRepository playerMouseViewRepository) {
            this.playerMouseStockRepository = playerMouseStockRepository;
            this.playerMouseViewRepository = playerMouseViewRepository;

            this.mouseCreateService = new MouseCreateService(playerMouseViewRepository);
            this.mouseRemoveService = new MouseRemoveService(playerMouseViewRepository);
            this.mouseChangeTransformService = new MouseChangeTransformService(playerMouseViewRepository);
            this.mouseChangeStateService = new MouseChangeStateService(playerMouseViewRepository);
            this.mouseChangeMakingAmountService = new MouseChangeMakingAmountService(playerMouseViewRepository);

            this.FetchMouseStockCount();

            this.root = root;
            this.orderedMouse = new List<Mouse> ();
        }

        public void FetchMouseStockCount () {
            // 現在のネズミ数を取得
            var playerMouseStockModel =  playerMouseStockRepository.GetOwn();
            this.mouseStockCount = playerMouseStockModel.MouseStockCount;
        }

        public void OrderMouse (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert (this.IsConsume (new MouseOrderAmount(1)), "Mouse のオーダー数が限界です");

            // ネズミを家からインスタンス化して特定位置にオーダーする（ねずみの種類は動的に変えられるようにしてもいいかも）
            var mousePrefab = ResourceLoader.LoadModel ("mouse_normal");
            var mouseInstance = Object.AppearToFloor (mousePrefab, root, GameManager.Instance.MouseHomeManager.HomePostion);
            var mouse = mouseInstance.GetComponent<Mouse> ();

            Debug.Assert (mouse != null, "Mouse コンポーネントがありません");
            Debug.Assert (!mouse.IsOrdered (), "すでにオーダーされています。");

            var makingPrefab = ResourceLoader.LoadPrefab ("Model/Making");
            mouse.OrderMaking (
                this.mouseCreateService.Execute(mouse.transform.position, arrangementTarget.PlayerArrangementTargetModel),
                new PreMono (mouse, makingPrefab, arrangementTarget.MonoInfo));
            orderedMouse.Add (mouse);
        }

        public void ReOrderMouse (PlayerMouseViewModel playerMouseViewModel) {

            // ネズミを家からインスタンス化して特定位置にオーダーする（ねずみの種類は動的に変えられるようにしてもいいかも）
            var mousePrefab = ResourceLoader.LoadModel ("mouse_normal");
            var mouseInstance = Object.AppearToFloor (mousePrefab, root, playerMouseViewModel.Position);
            mouseInstance.transform.rotation = Quaternion.Euler(playerMouseViewModel.Rotation);
            var mouse = mouseInstance.GetComponent<Mouse> ();

            Debug.Assert (mouse != null, "Mouse コンポーネントがありません");
            Debug.Assert (!mouse.IsOrdered (), "すでにオーダーされています。");

            var makingPrefab = ResourceLoader.LoadPrefab ("Model/Making");
            
            mouse.ReOrderMaking (
                playerMouseViewModel,
                new PreMono (mouse, makingPrefab, playerMouseViewModel.PlayerArrangementTargetModel.MonoInfo));
            orderedMouse.Add (mouse);
        }        

        public void ChangeState (Mouse mouse, IState state) {
            MouseViewState mouseViewState = MouseViewState.None;
            if (state is MoveToTarget) {
                mouseViewState = MouseViewState.Move;
            }
            else if (state is MakingState) {
                mouseViewState = MouseViewState.Making;
            }
            else if (state is BackToHomeState) {
                mouseViewState = MouseViewState.BackToHome;
            }
            else {
                Debug.Assert(false, "状態が不定です。");
            }
            this.mouseChangeStateService.Execute(mouse.PlayerMouseViewModel, mouseViewState);
        }

        public void ChangeTransform (Mouse mouse) {
            this.mouseChangeTransformService.Execute(mouse.PlayerMouseViewModel, mouse.transform.position, mouse.transform.rotation.eulerAngles);
        }

        public void ChangeMakingAmount (Mouse mouse, MakingAmount makingAmount) {
            this.mouseChangeMakingAmountService.Execute(mouse.PlayerMouseViewModel, makingAmount);
        }

        public void BackMouse (Mouse mouse) {
            Debug.Assert (orderedMouse.IndexOf (mouse) >= 0, "オーダー中のネズミではありません。");

            this.mouseRemoveService.Execute(mouse.PlayerMouseViewModel);
            
            orderedMouse.Remove (mouse);
            Object.DisAppear (mouse.gameObject);
            mouse.ClearDisposable();
        }
    }
}