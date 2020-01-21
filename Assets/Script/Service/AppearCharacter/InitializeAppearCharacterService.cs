using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InitializeAppearCharacterService
    {
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public InitializeAppearCharacterService(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;            
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute() {

            // 予約の追加
            foreach (var playerAppearCharacterReserveModel in this.playerAppearCharacterReserveRepository.GetAll())
            {
                GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(playerAppearCharacterReserveModel);
            }

            // Viewの追加
            foreach (var playerAppearCharacterViewModel in this.playerAppearCharacterViewRepository.GetAll())
            {
                var generator = new AppearCharacterGenerator(playerAppearCharacterViewModel.AppearCharacterModel);
                var appearCharacterViewModel = generator.Generate(playerAppearCharacterViewModel);
                GameManager.Instance.AppearCharacterManager.EnqueueRegister(appearCharacterViewModel);
            }
        }
    }   
}
