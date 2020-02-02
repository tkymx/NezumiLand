using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// デッキを新規作成する
    /// </summary>
    public class ParkOpenDeckCreateService 
    {
        private readonly IPlayerParkOpenDeckRepository playerParkOpenDeckRepository = null;

        public ParkOpenDeckCreateService(IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.playerParkOpenDeckRepository = playerParkOpenDeckRepository;
        }

        public PlayerParkOpenDeckModel Execute() {
            return this.playerParkOpenDeckRepository.Create();
        }
    }
}
