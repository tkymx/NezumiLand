using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class AppearParkOpenCharacterDirectorEntry : EntryBase {
        public uint AppearCharacterId;
    }

    public interface IAppearParkOpenCharacterDirectorRepository {
        IEnumerable<AppearParkOpenCharacterDirectorModel> GetAll ();
        AppearParkOpenCharacterDirectorModel Get (uint id);
    }

    public class AppearParkOpenCharacterDirectorRepository : RepositoryBase<AppearParkOpenCharacterDirectorEntry>, IAppearParkOpenCharacterDirectorRepository {

        private readonly IAppearCharacterRepository appearCharacterRepository = null;

        public AppearParkOpenCharacterDirectorRepository (IAppearCharacterRepository appearCharacterRepository, ContextMap contextMap) : base (contextMap.AppearParkOpenCharacterDirectorEntrys) { 
            this.appearCharacterRepository = appearCharacterRepository;
        }

        public AppearParkOpenCharacterDirectorModel CreateFromEntry (AppearParkOpenCharacterDirectorEntry entry)
        {
            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);
            Debug.Assert(appearCharacterModel != null, "appearCharacterModel が存在しません " + entry.AppearCharacterId);

            return new AppearParkOpenCharacterDirectorModel (
                entry.Id,
                appearCharacterModel);            
        }

        public IEnumerable<AppearParkOpenCharacterDirectorModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public AppearParkOpenCharacterDirectorModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}