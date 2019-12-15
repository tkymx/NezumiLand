using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class PlayerOnegaiEntry : EntryBase {
        [DataMember]
        public uint OnegaiId { get; set; }

        [DataMember]
        public string OnegaiState { get; set; }

        [DataMember]
        public float StartOnegaiTime { get; set; }
    }

    public interface IPlayerOnegaiRepository {
        IEnumerable<PlayerOnegaiModel> GetAll ();
        IEnumerable<PlayerOnegaiModel> GetAlreadyClose ();
        IEnumerable<PlayerOnegaiModel> GetDisplayable ();
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
            IOnegaiRepository onegaiRepository = OnegaiRepository.GetRepository (contextMap);
            return new PlayerOnegaiRepository (onegaiRepository, playerContextMap);
        }

        private PlayerOnegaiModel GeneratePlayerOnegaiModel(PlayerOnegaiEntry entry) {
            return new PlayerOnegaiModel (entry.Id, onegaiRepository.Get (entry.Id), entry.OnegaiState.ToString (), entry.StartOnegaiTime);
        }

        public IEnumerable<PlayerOnegaiModel> GetAll () {
            return this.entrys
                .Select (entry => GeneratePlayerOnegaiModel(entry));
        }

        public IEnumerable<PlayerOnegaiModel> GetAlreadyClose () {
            return GetDisplayable()
                .Where (model => model.IsClose(GameManager.Instance.TimeManager.ElapsedTime));
        }

        public IEnumerable<PlayerOnegaiModel> GetDisplayable () {
            return this.entrys
                .Where(entry => entry.OnegaiState != OnegaiState.Lock.ToString())
                .Select (entry => GeneratePlayerOnegaiModel(entry));
        }

        public PlayerOnegaiModel GetById (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {

                var onegaiModel = this.onegaiRepository.Get(id);
                Debug.Assert(onegaiModel != null, "OnegaiModel がありません : " + id.ToString());

                var playerOnegaiModel = new PlayerOnegaiModel(
                    id,
                    onegaiModel,
                    OnegaiState.Lock.ToString(),
                    GameManager.Instance.TimeManager.ElapsedTime // なんか微妙な気もするが
                );
                return playerOnegaiModel;
            }
            return GeneratePlayerOnegaiModel(foundEntry);
        }

        public List<PlayerOnegaiModel> GetByIds (List<uint> ids) {
            return ids
                .Select(id => GetById(id))
                .ToList();
        }

        private IEnumerable<PlayerOnegaiModel> GetClear () {
            return this.entrys
                .Select (entry => GeneratePlayerOnegaiModel(entry))
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
            var entry = this.GetEntry(playerOnegaiModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerOnegaiEntry () {
                    Id = playerOnegaiModel.Id,
                    OnegaiId = playerOnegaiModel.OnegaiModel.Id,
                    OnegaiState = playerOnegaiModel.OnegaiState.ToString (),
                    StartOnegaiTime = playerOnegaiModel.StartOnegaiTime
                };
            } else {
                this.entrys.Add(new PlayerOnegaiEntry () {
                    Id = playerOnegaiModel.Id,
                    OnegaiId = playerOnegaiModel.OnegaiModel.Id,
                    OnegaiState = playerOnegaiModel.OnegaiState.ToString (),
                    StartOnegaiTime = playerOnegaiModel.StartOnegaiTime
                });
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}