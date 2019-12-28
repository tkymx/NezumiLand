
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InitializeOrderedMouseService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public InitializeOrderedMouseService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public void Execute() {
            foreach (var playerMouseViewModel in this.playerMouseViewRepository.GetAll())
            {
                GameManager.Instance.MouseStockManager.ReOrderMouse(playerMouseViewModel);
            }
        }
    }    
}

