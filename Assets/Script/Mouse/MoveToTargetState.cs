using UnityEngine;
using System.Collections;

namespace NL
{
    public class MoveToTarget : IState
    {
        private Mouse context;        
        private IArrangementTarget targetObject;

        public MoveToTarget(Mouse context, IArrangementTarget targetObject)
        {
            this.context = context;
            this.targetObject = targetObject;
        }

        public void onEnter()
        {

        }

        private bool isAlivable()
        {
            return ObjectComparison.Distance(context.transform.position,targetObject.GetCenterPosition()) < targetObject.GetRange();
        }

        public IState onUpdate()
        {
            context.MoveTimeTo(targetObject.GetCenterPosition());

            // 到着したとき
            if (isAlivable())
            {
                // 物があれば作成する
                if (context.HasPreMono)
                {
                    return new MakingState(context, targetObject.GetCenterPosition());
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
