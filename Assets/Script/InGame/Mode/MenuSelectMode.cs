using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MenuSelectMode : IGameMode
    {
        private MenuSelectModeContext context;

        public MenuSelectMode(MenuSelectModeContext context)
        {
            this.context = context;
        }

        public void OnEnter()
        {
            GameManager.Instance.GameUIManager.MonoListPresenter.Show();
            this.context.RemoveSelectMonoInfo();
        }
        public void OnUpdate()
        {
            if (this.context.HasMonoInfo)
            {
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(GameModeGenerator.GenerateArrangementMode());
            }
        }
        public void OnExit()
        {
            GameManager.Instance.GameUIManager.MonoListPresenter.Close();
            this.context.RemoveSelectMonoInfo();
        }
    }
}