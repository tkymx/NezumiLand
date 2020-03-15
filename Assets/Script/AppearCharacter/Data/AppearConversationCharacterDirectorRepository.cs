using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class AppearConversationCharacterDirectorEntry : EntryBase {
        public uint AppearCharacterId;
        public uint ConversationId;
        public uint RewardId;
    }

    public interface IAppearConversationCharacterDirectorRepository {
        IEnumerable<AppearConversationCharacterDirectorModel> GetAll ();
        AppearConversationCharacterDirectorModel Get (uint id);
    }

    public class AppearConversationCharacterDirectorRepository : RepositoryBase<AppearConversationCharacterDirectorEntry>, IAppearConversationCharacterDirectorRepository {

        private readonly IAppearCharacterRepository appearCharacterRepository = null;
        private readonly IConversationRepository conversationRepository = null;
        private readonly IRewardRepository rewardRepository = null;

        public AppearConversationCharacterDirectorRepository (IAppearCharacterRepository appearCharacterRepository, IConversationRepository conversationRepository, IRewardRepository rewardRepository, ContextMap contextMap) : base (contextMap.AppearConversationCharacterDirectorEntrys) { 
            this.appearCharacterRepository = appearCharacterRepository;
            this.conversationRepository = conversationRepository;
            this.rewardRepository = rewardRepository;            
        }

        public AppearConversationCharacterDirectorModel CreateFromEntry (AppearConversationCharacterDirectorEntry entry)
        {
            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);
            Debug.Assert(appearCharacterModel != null, "appearCharacterModel が存在しません " + entry.AppearCharacterId);

            var conversationModel = this.conversationRepository.Get(entry.ConversationId);
            Debug.Assert(conversationModel != null, "conversationModel が存在しません " + entry.ConversationId);

            var rewardModel = this.rewardRepository.Get(entry.RewardId);
            Debug.Assert(rewardModel != null, "rewardModel が存在しません " + entry.RewardId);

            return new AppearConversationCharacterDirectorModel (
                entry.Id,
                appearCharacterModel,
                conversationModel,
                rewardModel);            
        }

        public IEnumerable<AppearConversationCharacterDirectorModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public AppearConversationCharacterDirectorModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}