using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterCreateService
    {
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;
        private readonly IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository;
        private readonly IPlayerAppearPlayingCharacterDirectorRepository playerAppearPlayingCharacterDirectorRepository;
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterCreateService(
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository, 
            IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository,
            IPlayerAppearPlayingCharacterDirectorRepository playerAppearPlayingCharacterDirectorRepository,
            IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;
            this.playerAppearOnegaiCharacterDirectorRepository = playerAppearOnegaiCharacterDirectorRepository;
            this.playerAppearPlayingCharacterDirectorRepository = playerAppearPlayingCharacterDirectorRepository;
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public PlayerAppearCharacterViewModel Execute(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            MovePath movePath,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            AppearCharacterDirectorModelBase appearCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel
        ) {
            
            PlayerAppearCharacterDirectorModelBase directorModel = null;
            switch(appearCharacterLifeDirectorType)
            {
                case AppearCharacterLifeDirectorType.Conversation :
                {
                    var appearConversationCharacterDirectorModel = appearCharacterDirectorModel as AppearConversationCharacterDirectorModel;
                    if (appearConversationCharacterDirectorModel != null)
                    {
                        directorModel = this.playerAppearConversationCharacterDirectorRepository.Create(
                            appearConversationCharacterDirectorModel,
                            playerAppearCharacterReserveModel);
                    }
                    break;
                }
                case AppearCharacterLifeDirectorType.Onegai :
                {
                    var appearOnegaiCharacterDirectorModel = appearCharacterDirectorModel as AppearOnegaiCharacterDirectorModel;
                    if (appearOnegaiCharacterDirectorModel != null)
                    {
                        directorModel = this.playerAppearOnegaiCharacterDirectorRepository.Create(
                            appearOnegaiCharacterDirectorModel,
                            playerAppearCharacterReserveModel);
                    }
                    break;
                }
                case AppearCharacterLifeDirectorType.Playing :
                {
                    var appearPlayingCharacterDirectorModel = appearCharacterDirectorModel as AppearPlayingCharacterDirectorModel;
                    if (appearPlayingCharacterDirectorModel != null)
                    {
                        directorModel = this.playerAppearPlayingCharacterDirectorRepository.Create(
                            appearPlayingCharacterDirectorModel,
                            playerAppearCharacterReserveModel);
                    }
                    break;
                }
            }

            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                appearCharacterLifeDirectorType,
                directorModel,
                movePath
            );
        }       
    }   
}
