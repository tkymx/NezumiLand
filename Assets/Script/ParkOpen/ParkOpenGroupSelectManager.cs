using UnityEngine;
using System;

namespace NL
{
    /// <summary>
    /// 開放するグループを選択に関する処理の集約
    /// </summary>
    public class ParkOpenGroupSelectManager 
    {
        private readonly IParkOpenGroupsRepository parkOpenGroupsRepository;

        public ParkOpenGroupSelectManager(IParkOpenGroupsRepository parkOpenGroupsRepository)
        {
            this.parkOpenGroupsRepository = parkOpenGroupsRepository;
        }

        /// <summary>
        /// 選択の開始
        /// </summary>
        public void StartSelect()
        {
            // はじめのグループを選択対象として表示する
            GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.Show();
            GameManager.Instance.GameUIManager.ParkOpenGroupsPresenter.SetContents(this.parkOpenGroupsRepository.Get(1));
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateParkOpenGroupSelectMode());
        }
    }
}