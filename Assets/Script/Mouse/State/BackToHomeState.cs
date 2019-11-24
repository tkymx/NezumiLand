using System.Collections;
using UnityEngine;

namespace NL {
    public class BackToHomeState : IState {
        private Mouse context;

        public BackToHomeState (Mouse context) {
            this.context = context;
        }

        public void onEnter () {

        }

        private bool isAlivable () {
            return ObjectComparison.Distance (context.transform.position, GameManager.Instance.MouseHomeManager.HomePostion) < GameManager.Instance.MouseHomeManager.HomeRange;
        }

        public IState onUpdate () {
            context.MoveTimeTo (GameManager.Instance.MouseHomeManager.HomePostion);

            // 到着したとき
            if (isAlivable ()) {
                GameManager.Instance.MouseStockManager.BackMouse (this.context);
                return new EmptyState ();
            }
            return null;
        }

        public void onExit () {

        }
    }
}