using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearCharacterReserveEntry : EntryBase 
    {        
        public string AppearCharacterLifeDirectorType;
        public uint AppearCharacterDirectorId;
        public string State;
        public float Rate;
        public bool IsNextRemove;
        public bool IsNextSkip;
    }

    public interface IPlayerAppearCharacterReserveRepository 
    {
        PlayerAppearCharacterReserveModel Get (uint id);        
        List<PlayerAppearCharacterReserveModel> GetAll ();      
        PlayerAppearCharacterReserveModel Create (           
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition);
        void Store (PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
        void Remove (PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
    }

    public class PlayerAppearCharacterReserveRepository : PlayerRepositoryBase<PlayerAppearCharacterReserveEntry>, IPlayerAppearCharacterReserveRepository {

        private enum Condition { 
            None,
            ByChance,
            Force
        }

        private readonly IAppearConversationCharacterDirectorRepository appearConversationCharacterDirectorRepository;
        private readonly IAppearOnegaiCharacterDirectorRepository appearOnegaiCharacterDirectorRepository;

        public PlayerAppearCharacterReserveRepository (
            IAppearConversationCharacterDirectorRepository appearConversationCharacterDirectorRepository,
            IAppearOnegaiCharacterDirectorRepository appearOnegaiCharacterDirectorRepository,
            PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearCharacterReserveEntrys) 
        {
            this.appearConversationCharacterDirectorRepository = appearConversationCharacterDirectorRepository;
            this.appearOnegaiCharacterDirectorRepository = appearOnegaiCharacterDirectorRepository;
        }

        private PlayerAppearCharacterReserveModel CreateByEntry (PlayerAppearCharacterReserveEntry entry) {

            var lifeType = AppearCharacterLifeDirectorType.None;
            if (Enum.TryParse (entry.AppearCharacterLifeDirectorType, out AppearCharacterLifeDirectorType outLifeType)) {
                lifeType = outLifeType;
            }

            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase = null;
            switch (lifeType) {
                case AppearCharacterLifeDirectorType.Conversation: 
                {
                    appearCharacterDirectorModelBase = this.appearConversationCharacterDirectorRepository.Get(entry.AppearCharacterDirectorId);
                    Debug.Assert(appearCharacterDirectorModelBase != null, "Reserveのモデル が存在しません " + entry.AppearCharacterDirectorId);
                    break;
                }
                case AppearCharacterLifeDirectorType.Onegai:
                {
                    appearCharacterDirectorModelBase = this.appearOnegaiCharacterDirectorRepository.Get(entry.AppearCharacterDirectorId);
                    Debug.Assert(appearCharacterDirectorModelBase != null, "Onegaiのモデルが 存在しません" + entry.AppearCharacterDirectorId);
                    break;
                }
                default: 
                {
                    Debug.Assert(false, "存在しないタイプが指定されました。 " + lifeType.ToString());
                    break;
                }
            }

            var state = Condition.None;
            if (Enum.TryParse (entry.State, out Condition outState)) {
                state = outState;
            }

            IDailyAppearCharacterRegistCondition condition = null;
            if (state == Condition.ByChance) {
                condition = new DailyAppearCharacterRegistConditionByChance(entry.Rate);
            } else if (state == Condition.Force) {
                condition = new DailyAppearCharacterRegistConditionForce();
            } else {
                Debug.Assert(false, "街燈する state がありません");
            }

            return new PlayerAppearCharacterReserveModel(
                entry.Id,
                lifeType,
                appearCharacterDirectorModelBase,
                condition,
                entry.IsNextRemove,
                entry.IsNextSkip
            );
        }

        public PlayerAppearCharacterReserveModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearCharacterReserveModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearCharacterReserveModel Create (
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition
        ) {
            var id = this.MaximuId()+1;

            var condition = Condition.None;
            var rate = 0.0f;
            if (dailyAppearCharacterRegistCondition is DailyAppearCharacterRegistConditionForce) {
                condition = Condition.Force;
            } else if (dailyAppearCharacterRegistCondition is DailyAppearCharacterRegistConditionByChance) {
                condition = Condition.ByChance;
                rate = (dailyAppearCharacterRegistCondition as DailyAppearCharacterRegistConditionByChance).Rate;
            } else {
                Debug.Assert(false, "条件が見つかりません");
            }

            var entry = new PlayerAppearCharacterReserveEntry () {
                Id = id,
                AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType.ToString(),
                AppearCharacterDirectorId = appearCharacterDirectorModelBase.Id, 
                State = condition.ToString(),
                Rate = rate,
                IsNextRemove = false,
                IsNextSkip = false
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            var entry = this.GetEntry(playerAppearCharacterReserveModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);

                var condition = Condition.None;
                var rate = 0.0f;
                if (playerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition is DailyAppearCharacterRegistConditionForce) {
                    condition = Condition.Force;
                } else if (playerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition is DailyAppearCharacterRegistConditionByChance) {
                    condition = Condition.ByChance;
                    rate = (playerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition as DailyAppearCharacterRegistConditionByChance).Rate;
                } else {
                    Debug.Assert(false, "条件が見つかりません");
                }

                this.entrys[index] = new PlayerAppearCharacterReserveEntry () {
                    Id = playerAppearCharacterReserveModel.Id,
                    AppearCharacterLifeDirectorType = playerAppearCharacterReserveModel.AppearCharacterLifeDirectorType.ToString(),
                    AppearCharacterDirectorId = playerAppearCharacterReserveModel.AppearCharacterDirectorModelBase.Id,
                    State = condition.ToString(),
                    Rate = rate,
                    IsNextRemove = playerAppearCharacterReserveModel.IsNextRemove,
                    IsNextSkip = playerAppearCharacterReserveModel.IsNextSkip
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerAppearCharacterReserveModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            var entry = this.GetEntry(playerAppearCharacterReserveModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerAppearCharacterReserveModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
