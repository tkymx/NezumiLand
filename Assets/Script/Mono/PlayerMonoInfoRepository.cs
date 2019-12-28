using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class PlayerMonoInfoEntry : EntryBase {
        
        public uint MonoInfoId;

        
        public bool IsRelease;
    }

    public interface IPlayerMonoInfoRepository {
        PlayerMonoInfo GetById (uint id);
        IEnumerable<PlayerMonoInfo> GetDisplayableByType (MonoType type);
        void Store (PlayerMonoInfo playerMonoInfo);
    }

    /// <summary>
    /// プレイヤーのものの状態を管理
    /// - 現状は解放条件のみ、今後はランクなどが増えてもいいと思う
    /// </summary>
    public class PlayerMonoInfoRepository : PlayerRepositoryBase<PlayerMonoInfoEntry>, IPlayerMonoInfoRepository {
        private readonly IMonoInfoRepository monoInfoRepository;

        public PlayerMonoInfoRepository (IMonoInfoRepository monoInfoRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerMonoInfoEntrys) {
            this.monoInfoRepository = monoInfoRepository;
        }

        public static PlayerMonoInfoRepository GetRepository (ContextMap contextMap, PlayerContextMap playerContextMap) {
            IMonoInfoRepository MonoInfoRepository = new MonoInfoRepository (contextMap);
            return new PlayerMonoInfoRepository (MonoInfoRepository, playerContextMap);
        }

        public PlayerMonoInfo GetById (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {

                var MonoInfo = this.monoInfoRepository.Get(id);
                Debug.Assert(MonoInfo != null, "MonoInfo がありません : " + id.ToString());

                var playerMonoInfo = new PlayerMonoInfo(
                    id,
                    MonoInfo,
                    false
                );
                return playerMonoInfo;
            }
            return new PlayerMonoInfo (foundEntry.Id, monoInfoRepository.Get (foundEntry.Id), foundEntry.IsRelease);
        }

        public IEnumerable<PlayerMonoInfo> GetDisplayableByType (MonoType type) {
            return this.monoInfoRepository.GetByType(type)
                .Select(monoInfo => this.GetById(monoInfo.Id));
        }


        public void Store (PlayerMonoInfo playerMonoInfo) {
            var entry = this.GetEntry(playerMonoInfo.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerMonoInfoEntry () {
                    Id = playerMonoInfo.Id,
                    MonoInfoId = playerMonoInfo.MonoInfo.Id,
                    IsRelease = playerMonoInfo.IsRelease
                };
            } else {
                this.entrys.Add(new PlayerMonoInfoEntry () {
                    Id = playerMonoInfo.Id,
                    MonoInfoId = playerMonoInfo.MonoInfo.Id,
                    IsRelease = playerMonoInfo.IsRelease
                });
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}
