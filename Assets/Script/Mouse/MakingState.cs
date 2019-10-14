using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MakingState : IState
    {
        private Mouse context;
        private Vector3 makingPosition;

        private float elapsedTime = 0;

        public MakingState(Mouse context, Vector3 makingPosition)
        {
            this.context = context;
            this.makingPosition = makingPosition;
            this.elapsedTime = 0;
        }

        public void onEnter()
        {
            context.StartMake(makingPosition);
        }

        public IState onUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 2.0f)
            {
                context.FinishMaking(makingPosition);
                return new EmptyState();
            }

            return null;
        }

        public void onExit()
        {

        }
    }
}