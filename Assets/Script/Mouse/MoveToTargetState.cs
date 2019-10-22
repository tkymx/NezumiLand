using UnityEngine;
using System.Collections;

namespace NL
{
    public class MoveToTarget : IState
    {
        private Mouse context;        
        private GameObject targetObject;

        public MoveToTarget(Mouse context, GameObject targetObject)
        {
            this.context = context;
            this.targetObject = targetObject;
        }

        public void onEnter()
        {

        }

        private bool isAlivable()
        {
            return ObjectComparison.Distance(context.transform.position,targetObject.transform.position) < 1.0f;
        }

        public IState onUpdate()
        {
            context.MoveTimeTo(targetObject.transform.position);

            // 到着したとき
            if (isAlivable())
            {
                // 物があれば作成する
                if (context.HasPreMono)
                {
                    return new MakingState(context, targetObject.transform.position);
                }

                return new EmptyState();
            }
            return null;
        }

        public void onExit()
        {

        }
    }
}
