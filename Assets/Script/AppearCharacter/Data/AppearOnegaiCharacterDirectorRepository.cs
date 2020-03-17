using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class AppearOnegaiCharacterDirectorEntry : EntryBase {
        public uint AppearCharacterId;
        public uint AfterConversationId;
        public uint BeforeConversationId;
        public uint OnegaiId;
        public uint RewardId;
    }

    public interface IAppearOnegaiCharacterDirectorRepository {
        IEnumerable<AppearOnegaiCharacterDirectorModel> GetAll ();
        AppearOnegaiCharacterDirectorModel Get (uint id);
    }

    public class AppearOnegaiCharacterDirectorRepository : RepositoryBase<AppearOnegaiCharacterDirectorEntry>, IAppearOnegaiCharacterDirectorRepository {

        private readonly IAppearCharacterRepository appearCharacterRepository = null;
        private readonly IConversationRepository conversationRepository = null;
        private readonly IOnegaiRepository onegaiRepository = null;
        private readonly IRewardRepository rewardRepository = null;

        public AppearOnegaiCharacterDirectorRepository (IAppearCharacterRepository appearCharacterRepository, IConversationRepository conversationRepository, IOnegaiRepository onegaiRepository, IRewardRepository rewardRepository, ContextMap contextMap) : base (contextMap.AppearOnegaiCharacterDirectorEntrys) { 
            this.appearCharacterRepository = appearCharacterRepository;
            this.conversationRepository = conversationRepository;
            this.onegaiRepository = onegaiRepository;
            this.rewardRepository = rewardRepository;            
        }

        public AppearOnegaiCharacterDirectorModel CreateFromEntry (AppearOnegaiCharacterDirectorEntry entry)
        {
            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);
            Debug.Assert(appearCharacterModel != null, "appearCharacterModel が存在しません " + entry.AppearCharacterId);

            var afterConversationModel = this.conversationRepository.Get(entry.AfterConversationId);
            Debug.Assert(afterConversationModel != null, "afterConversationModel が存在しません " + entry.AfterConversationId);

            var beforeConversationModel = this.conversationRepository.Get(entry.BeforeConversationId);
            Debug.Assert(beforeConversationModel != null, "beforeConversationModel が存在しません " + entry.BeforeConversationId);

            var onegaiModel = this.onegaiRepository.Get(entry.OnegaiId);
            Debug.Assert(onegaiModel != null, "onegaiModel が存在しません " + entry.OnegaiId);

            var rewardModel = this.rewardRepository.Get(entry.RewardId);
            Debug.Assert(rewardModel != null, "rewardModel が存在しません " + entry.RewardId);

            return new AppearOnegaiCharacterDirectorModel (
                entry.Id,
                appearCharacterModel,
                afterConversationModel,
                beforeConversationModel,
                onegaiModel,
                rewardModel);            
        }

        public IEnumerable<AppearOnegaiCharacterDirectorModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public AppearOnegaiCharacterDirectorModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}