using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// カードを生成する。取得する際に使用
    /// </summary>
    public class ParkOpenCardCreateService 
    {
        private readonly IPlayerParkOpenCardRepository playerParkOpenCardRepository = null;

        public ParkOpenCardCreateService(IPlayerParkOpenCardRepository playerParkOpenCardRepository)
        {
            this.playerParkOpenCardRepository = playerParkOpenCardRepository;
        }

        public PlayerParkOpenCardModel Execute(ParkOpenCardModel parkOpenCardModel) {
            return this.playerParkOpenCardRepository.Create(parkOpenCardModel);
        }
    }    
}
