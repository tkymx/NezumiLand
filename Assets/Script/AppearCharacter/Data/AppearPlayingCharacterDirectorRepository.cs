using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class AppearPlayingCharacterDirectorEntry : EntryBase {
        public uint AppearCharacterId;
    }

    public interface IAppearPlayingCharacterDirectorRepository {
        IEnumerable<AppearPlayingCharacterDirectorModel> GetAll ();
        AppearPlayingCharacterDirectorModel Get (uint id);
    }

    public class AppearPlayingCharacterDirectorRepository : RepositoryBase<AppearPlayingCharacterDirectorEntry>, IAppearPlayingCharacterDirectorRepository {

        private readonly IAppearCharacterRepository appearCharacterRepository = null;

        public AppearPlayingCharacterDirectorRepository (IAppearCharacterRepository appearCharacterRepository, ContextMap contextMap) : base (contextMap.AppearPlayingCharacterDirectorEntrys) { 
            this.appearCharacterRepository = appearCharacterRepository;
        }

        public AppearPlayingCharacterDirectorModel CreateFromEntry (AppearPlayingCharacterDirectorEntry entry)
        {
            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);
            Debug.Assert(appearCharacterModel != null, "appearCharacterModel が存在しません " + entry.AppearCharacterId);

            return new AppearPlayingCharacterDirectorModel (
                entry.Id,
                appearCharacterModel);            
        }

        public IEnumerable<AppearPlayingCharacterDirectorModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public AppearPlayingCharacterDirectorModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}