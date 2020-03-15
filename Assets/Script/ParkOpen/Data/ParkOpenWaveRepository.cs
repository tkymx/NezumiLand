using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenWaveEntry : EntryBase {
        public int AppearCount;
        public int FluctuationCount;
        public uint[] AppearParkOpenCharacterDirectorIds;
    }

    public interface IParkOpenWaveRepository {
        IEnumerable<ParkOpenWaveModel> GetAll ();
        ParkOpenWaveModel Get (uint id);
    }

    public class ParkOpenWaveRepository : RepositoryBase<ParkOpenWaveEntry>, IParkOpenWaveRepository {

        private IAppearParkOpenCharacterDirectorRepository appearParkOpenCharacterDirectorRepository;

        public ParkOpenWaveRepository (IAppearParkOpenCharacterDirectorRepository appearParkOpenCharacterDirectorRepository, ContextMap contextMap) : base (contextMap.ParkOpenWaveEntrys) {
            this.appearParkOpenCharacterDirectorRepository = appearParkOpenCharacterDirectorRepository;
        }

        public ParkOpenWaveModel CreateFromEntry (ParkOpenWaveEntry entry)
        {
            var appearParkOpenCharacterDirectorModels = entry.AppearParkOpenCharacterDirectorIds.Select(id => {
                var appearParkOpenCharacterDirectorModel = this.appearParkOpenCharacterDirectorRepository.Get(id);
                Debug.Assert(appearParkOpenCharacterDirectorModel != null, "appearParkOpenCharacterDirectorModel がありません。 " + id.ToString());
                return appearParkOpenCharacterDirectorModel;
            }); 

            return new ParkOpenWaveModel (
                entry.Id,
                entry.AppearCount,
                entry.FluctuationCount,
                appearParkOpenCharacterDirectorModels.ToArray());
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