using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class PlayerMonoViewEntry : EntryBase {
        
        public uint MonoInfoId;

        
        public int Level;
    }

    public interface IPlayerMonoViewRepository {
        PlayerMonoViewModel Get (uint id);
        PlayerMonoViewModel Create (uint monoId, int level);
        void Store (PlayerMonoViewModel playerMonoViewModel);
        void Remove (PlayerMonoViewModel playerMonoViewModel);
    }

    public class PlayerMonoViewRepository : PlayerRepositoryBase<PlayerMonoViewEntry>, IPlayerMonoViewRepository {
        private readonly IMonoInfoRepository monoInfoRepository;

        public PlayerMonoViewRepository (IMonoInfoRepository monoInfoRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerMonoViewEntrys) {
            this.monoInfoRepository = monoInfoRepository;
        }

        public static PlayerMonoViewRepository GetRepository (ContextMap contextMap, PlayerContextMap playerContextMap) {
            IMonoInfoRepository monoInfoRepository = new MonoInfoRepository (contextMap);
            return new PlayerMonoViewRepository (monoInfoRepository, playerContextMap);
        }

        public PlayerMonoViewModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return new PlayerMonoViewModel (foundEntry.Id, monoInfoRepository.Get (foundEntry.MonoInfoId), foundEntry.Level);
        }

        public PlayerMonoViewModel Create (uint monoId, int level) {
            var monoInfo = this.monoInfoRepository.Get(monoId);
            Debug.Assert(monoInfo != null, "monoInfo がありません : " + monoId.ToString());

            var id = this.MaximuId()+1;

            this.entrys.Add(new PlayerMonoViewEntry () {
                Id = id,
                MonoInfoId = monoId,
                Level = level
            });

            PlayerContextMap.WriteEntry (this.entrys);

            var playerMonoView = new PlayerMonoViewModel(
                id,
                monoInfo,
                level
            );

            return playerMonoView;
        }

        public void Store (PlayerMonoViewModel playerMonoViewModel) {
            var entry = this.GetEntry(playerMonoViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerMonoViewEntry () {
                    Id = playerMonoViewModel.Id,
                    MonoInfoId = playerMonoViewModel.MonoInfo.Id,
                    Level = playerMonoViewModel.Level
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerMonoViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerMonoViewModel playerMonoViewModel) {
            var entry = this.GetEntry(playerMonoViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerMonoViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
