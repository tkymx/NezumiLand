using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterCreateService
    {
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;
        private readonly IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository;
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterCreateService(
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository, 
            IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository,
            IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;
            this.playerAppearOnegaiCharacterDirectorRepository = playerAppearOnegaiCharacterDirectorRepository;
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public PlayerAppearCharacterViewModel Execute(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            PlayerAppearCharacterDirectorModelBase playerAppearCharacterDirectorModelBase,
            MovePath movePath
        ) {
            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                appearCharacterLifeDirectorType,
                playerAppearCharacterDirectorModelBase,
                movePath
            );
        }

        public PlayerAppearCharacterViewModel ExecuteWithConversatoinDirector(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            MovePath movePath,
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel
        ) {
            var directorModel = this.playerAppearConversationCharacterDirectorRepository.Create(
                appearConversationCharacterDirectorModel,
                playerAppearCharacterReserveModel);

            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                AppearCharacterLifeDirectorType.Conversation,
                directorModel,
                movePath
            );
        }

        public PlayerAppearCharacterViewModel ExecuteWithOnegaiDirector(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            MovePath movePath,
            AppearOnegaiCharacterDirectorModel appearOnegaiCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel
        ) {
            var directorModel = this.playerAppearOnegaiCharacterDirectorRepository.Create(
                appearOnegaiCharacterDirectorModel,
                playerAppearCharacterReserveModel);

            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                AppearCharacterLifeDirectorType.Onegai,
                directorModel,
                movePath
            );
        }
    }   
}
