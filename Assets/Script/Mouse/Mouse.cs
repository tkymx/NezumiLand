using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    struct MouseParameter
    {
        public float speed;
    }

    public class Mouse : MonoBehaviour
    {
        [SerializeField]
        private SimpleAnimation simpleAnimation = null;

        /// <summary>
        /// ネズミの行動の状態遷移
        /// </summary>
        private StateManager stateManager;

        private MouseParameter mouseParameter;

        private Vector3 moveVector;

        private PreMono currentPreMono;

        /// <summary>
        /// プレモノをっ持っているかどうか？
        /// </summary>
        public bool HasPreMono => currentPreMono != null;

        // Start is called before the first frame update
        void Start()
        {
            mouseParameter = new MouseParameter()
            {
                speed = 0.1f,
            };

            stateManager = new StateManager(new EmptyState());
            currentPreMono = null;
        }

        // Update is called once per frame
        void Update() 
        {
            InitializeByFrame();
            stateManager.UpdateByFrame();
            Move();
        }

        void InitializeByFrame()
        {
            moveVector = Vector3.zero;
        }

        void Move()
        {
            if (moveVector.magnitude > 0.001)
            {
                simpleAnimation.CrossFade("run", 0.5f);
                transform.rotation = Quaternion.LookRotation(moveVector);
                transform.position = transform.position + moveVector;
            }
            else
            {
                simpleAnimation.CrossFade("idle", 0.5f);
            }
        }

        public void StartMake(Vector3 position)
        {
            if (!HasPreMono)
            {
                Debug.LogError("プレモノを持っていないのにMakeが呼ばれました");
            }
            currentPreMono.StartMaking(position);
        }

        public void FinishMaking(Vector3 position)
        {
            if (!HasPreMono)
            {
                Debug.LogError("プレモノを持っていないのにMakeが呼ばれました");
            }
            currentPreMono.FinishMaking(position);
            currentPreMono = null;
        }

        // 移動のみを行う
        public void MoveTimeTo(Vector3 target)
        {
            moveVector = ObjectComparison.Direction(target,transform.position) * this.mouseParameter.speed;
        }

        public void OrderMaking(IArrangementTarget targetObject, PreMono preMono)
        {
            this.currentPreMono = preMono;
            stateManager.Interrupt(new MoveToTarget(this, targetObject));
        }
    }
}