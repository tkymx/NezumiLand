﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class AppearCharacterEntry : EntryBase {

        
        public string Name;
    }

    public interface IAppearCharacterRepository {
        IEnumerable<AppearCharacterModel> GetAll ();
        AppearCharacterModel Get (uint id);
    }

    public class AppearCharacterRepository : RepositoryBase<AppearCharacterEntry>, IAppearCharacterRepository {
        public AppearCharacterRepository (ContextMap contextMap) : base (contextMap.AppearCharacterEntrys) { }

        public IEnumerable<AppearCharacterModel> GetAll () {
            return entrys.Select (entry => {
                return new AppearCharacterModel (
                    entry.Id,
                    entry.Name);
            });
        }

        public AppearCharacterModel Get (uint id) {
            var entry = base.GetEntry(id);
            return new AppearCharacterModel (
                entry.Id,
                entry.Name);
        }
    }
}