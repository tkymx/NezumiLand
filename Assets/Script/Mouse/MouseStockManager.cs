using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MouseStockManager : ConsumableCollectionBase<MouseOrderAmount> {
        private readonly IPlayerMouseStockRepository playerMouseStockRepository;

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

        public MouseStockManager (GameObject root, IPlayerMouseStockRepository playerMouseStockRepository) {
            this.playerMouseStockRepository = playerMouseStockRepository;
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
            mouse.OrderMaking (arrangementTarget, new PreMono (mouse, makingPrefab, arrangementTarget.MonoInfo));
            orderedMouse.Add (mouse);
        }

        public void BackMouse (Mouse mouse) {
            Debug.Assert (orderedMouse.IndexOf (mouse) >= 0, "オーダー中のネズミではありません。");
            orderedMouse.Remove (mouse);
            Object.DisAppear (mouse.gameObject);
        }
    }
}