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
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show();
            GameManager.Instance.GameUIManager.MonoTabPresenter.Show();
            GameManager.Instance.TimeManager.Pause();
        }
        public void OnUpdate()
        {
            if (!GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo)
            {
                return;
            }

            Debug.Assert(GameManager.Instance.MouseStockManager.IsOrderMouse, "オーダーできないのにモノが選択されました。");

            // なんかここで入れてあげるのは違う気もする
            this.context.SelectMonoInfo(GameManager.Instance.MonoSelectManager.SelectedMonoInfo);

            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateArrangementMode());
        }
        public void OnExit()
        {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close();
            GameManager.Instance.GameUIManager.MonoTabPresenter.Close();
            GameManager.Instance.TimeManager.Play();
        }
    }
}