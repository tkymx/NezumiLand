using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenWaveEntry : EntryBase {
        public int AppearCount;
        public int FluctuationCount;
        public uint[] AppearCharacterIds;
    }

    public interface IParkOpenWaveRepository {
        IEnumerable<ParkOpenWaveModel> GetAll ();
        ParkOpenWaveModel Get (uint id);
    }

    public class ParkOpenWaveRepository : RepositoryBase<ParkOpenWaveEntry>, IParkOpenWaveRepository {

        private IAppearCharacterRepository appearCharacterRepository;

        public ParkOpenWaveRepository (IAppearCharacterRepository appearCharacterRepository, ContextMap contextMap) : base (contextMap.ParkOpenWaveEntrys) {
            this.appearCharacterRepository = appearCharacterRepository;
        }

        public ParkOpenWaveModel CreateFromEntry (ParkOpenWaveEntry entry)
        {
            var appearCharacterModels = entry.AppearCharacterIds.Select(id => {
                var appearCharacterModel = this.appearCharacterRepository.Get(id);
                Debug.Assert(appearCharacterModel != null, "appearCharacterModelがありません。 " + id.ToString());
                return appearCharacterModel;
            }); 

            return new ParkOpenWaveModel (
                entry.Id,
                entry.AppearCount,
                entry.FluctuationCount,
                appearCharacterModels.ToArray());
        }

        public IEnumerable<ParkOpenWaveModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenWaveModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}