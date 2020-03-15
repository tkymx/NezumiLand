using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterCreateService
    {
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;
        private readonly IPlayerAppearParkOpenCharacterDirectorRepository playerAppearParkOpenCharacterDirectorRepository;
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterCreateService(
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository, 
            IPlayerAppearParkOpenCharacterDirectorRepository playerAppearParkOpenCharacterDirectorRepository,
            IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;
            this.playerAppearParkOpenCharacterDirectorRepository = playerAppearParkOpenCharacterDirectorRepository;
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

        public PlayerAppearCharacterViewModel ExecuteWithParkOpenDirector(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            MovePath movePath,
            AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel
        ) {
            var directorModel = this.playerAppearParkOpenCharacterDirectorRepository.Create(
                appearParkOpenCharacterDirectorModel);

            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                AppearCharacterLifeDirectorType.ParkOpen,
                directorModel,
                movePath
            );
        }        

    }   
}
