using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class PlayerOnegaiEntry {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public uint OnegaiId { get; set; }

        [DataMember]
        public string OnegaiState { get; set; }
    }

    public interface IPlayerOnegaiRepository {
        IEnumerable<PlayerOnegaiModel> GetAll ();
        PlayerOnegaiModel GetById (uint id);
        List<PlayerOnegaiModel> GetByIds (List<uint> ids);
        Satisfaction GetAllSatisfaction ();
        void Store (PlayerOnegaiModel playerOnegaiModel);
    }

    /// <summary>
    /// プレイヤーのお願いの状態を持っている
    /// 最終的には、持つだけではなく、保存まで行いたいが、現状は仕組みができていないので一時的な情報のみを保持する
    /// </summary>
    public class PlayerOnegaiRepository : PlayerRepositoryBase<PlayerOnegaiEntry>, IPlayerOnegaiRepository {
        private readonly IOnegaiRepository onegaiRepository;

        public PlayerOnegaiRepository (IOnegaiRepository onegaiRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerOnegaiEntrys) {
            this.onegaiRepository = onegaiRepository;
        }

        public static PlayerOnegaiRepository GetRepository (ContextMap contextMap, PlayerContextMap playerContextMap) {
            IOnegaiRepository onegaiRepository = new OnegaiRepository (contextMap);
            return new PlayerOnegaiRepository (onegaiRepository, playerContextMap);
        }

        public IEnumerable<PlayerOnegaiModel> GetAll () {
            return this.entrys
                .Select (entry => new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString ()));
        }

        public PlayerOnegaiModel GetById (uint id) {
            var foundEntrys = this.entrys.Where (entry => entry.Id == id);
            if (foundEntrys.Count() <= 0) {

                var onegaiModel = this.onegaiRepository.Get(id);
                Debug.Assert(onegaiModel != null, "OnegaiModel がありません : " + id.ToString());

                var playerOnegaiModel = new PlayerOnegaiModel(
                    id,
                    onegaiModel,
                    OnegaiState.Lock.ToString()
                );
                return playerOnegaiModel;
            }
            var foundEntry = foundEntrys.First();
            return new PlayerOnegaiModel (foundEntry.Id, onegaiRepository.Get (foundEntry.Id), foundEntry.OnegaiState.ToString ());
        }

        public List<PlayerOnegaiModel> GetByIds (List<uint> ids) {
            return ids
                .Select(id => GetById(id))
                .ToList();
        }

        private IEnumerable<PlayerOnegaiModel> GetClear () {
            return this.entrys
                .Select (entry => new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString ()))
                .Where (model => model.OnegaiState == OnegaiState.Clear);
        }

        public Satisfaction GetAllSatisfaction () {
            Satisfaction allSatisfaction = new Satisfaction (0);
            foreach (var playerOnegaiModel in this.GetClear ()) {
                allSatisfaction += playerOnegaiModel.OnegaiModel.Satisfaction;
            }
            return allSatisfaction;
        }

        public void Store (PlayerOnegaiModel playerOnegaiModel) {
            var entry = this.entrys.Find (e => e.Id == playerOnegaiModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerOnegaiEntry () {
                    Id = playerOnegaiModel.Id,
                    OnegaiId = playerOnegaiModel.OnegaiModel.Id,
                    OnegaiState = playerOnegaiModel.OnegaiState.ToString ()
                };
            } else {
                this.entrys.Add(new PlayerOnegaiEntry () {
                    Id = playerOnegaiModel.Id,
                    OnegaiId = playerOnegaiModel.OnegaiModel.Id,
                    OnegaiState = playerOnegaiModel.OnegaiState.ToString ()
                });
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}