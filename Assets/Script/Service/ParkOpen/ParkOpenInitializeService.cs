using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地開放がされていたら開始する
    /// </summary>
    public class ParkOpenInitializeService 
    {
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        public ParkOpenInitializeService(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void Execute() {
            var playerParkOpenModel = this.playerParkOpenRepository.GetOwn();
            if (playerParkOpenModel.IsOpen)
            {
                GameManager.Instance.ParkOpenManager.Open(playerParkOpenModel);
            }
        }
    }    
}

