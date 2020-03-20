using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearOnegaiCharacterService
    {
        private readonly IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository;

        public AppearOnegaiCharacterService(
            IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository)
        {
            this.playerAppearOnegaiCharacterDirectorRepository = playerAppearOnegaiCharacterDirectorRepository;         
        }

        // onegai
        public void Cancel(PlayerAppearOnegaiCharacterDirectorModel playerAppearOnegaiCharacterDirectorModel) {
            playerAppearOnegaiCharacterDirectorModel.Cancel();
            playerAppearOnegaiCharacterDirectorRepository.Store(playerAppearOnegaiCharacterDirectorModel);            
        }

    }   
}
