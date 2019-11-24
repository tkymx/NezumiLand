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
        IEnumerable<PlayerOnegaiModel> GetById (uint id);
        IEnumerable<PlayerOnegaiModel> GetByTriggerMonoInfoId (uint triggerMonoInfoId);
        IEnumerable<PlayerOnegaiModel> GetMediatable (OnegaiCondition onegaiCondition, uint targetMonoInfoId);
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
            foreach (var onegaiModel in this.onegaiRepository.GetAll ()) {
                if (this.entrys.Any (entry => entry.OnegaiId == onegaiModel.Id)) {
                    continue;
                }

                this.entrys.Add (new PlayerOnegaiEntry () {
                    Id = onegaiModel.Id,
                        OnegaiId = onegaiModel.Id,
                        OnegaiState = OnegaiState.UnLock.ToString ()
                });
            }
        }

        public static PlayerOnegaiRepository GetRepository (ContextMap contextMap, PlayerContextMap playerContextMap) {
            IOnegaiRepository onegaiRepository = new OnegaiRepository (contextMap);
            return new PlayerOnegaiRepository (onegaiRepository, playerContextMap);
        }

        public IEnumerable<PlayerOnegaiModel> GetAll () {
            return this.entrys
                .Select (entry => new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString ()));
        }

        public IEnumerable<PlayerOnegaiModel> GetById (uint id) {
            return this.entrys
                .Where (entry => entry.Id == id)
                .Select (entry => new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString ()));
        }

        public IEnumerable<PlayerOnegaiModel> GetByTriggerMonoInfoId (uint triggerMonoInfoId) {
            return this.entrys
                .Select (entry => new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString ()))
                .Where (model => model.OnegaiModel.TriggerMonoInfoId == triggerMonoInfoId);
        }

        public IEnumerable<PlayerOnegaiModel> GetMediatable (OnegaiCondition onegaiCondition, uint targetMonoInfoId) {
            return this.GetByTriggerMonoInfoId (targetMonoInfoId)
                .Where (model => model.OnegaiState == OnegaiState.UnLock)
                .Where (model => model.OnegaiModel.OnegaiCondition == onegaiCondition);
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
            var entry = this.entrys.Where (e => e.Id == playerOnegaiModel.Id).First ();
            Debug.Assert (entry != null, "保存対象が見つかりませんでした。");
            var index = this.entrys.IndexOf (entry);
            this.entrys[index] = new PlayerOnegaiEntry () {
                Id = playerOnegaiModel.Id,
                OnegaiId = playerOnegaiModel.OnegaiModel.Id,
                OnegaiState = playerOnegaiModel.OnegaiState.ToString ()
            };
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}