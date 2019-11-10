using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using UnityEngine;

namespace NL
{
    public interface IPlayerOnegaiRepository
    {
        IEnumerable<PlayerOnegaiModel> GetAll();
    }

    /// <summary>
    /// プレイヤーのお願いの状態を持っている
    /// 最終的には、持つだけではなく、保存まで行いたいが、現状は仕組みができていないので一時的な情報のみを保持する
    /// </summary>
    public class PlayerOnegaiRepository : IPlayerOnegaiRepository
    {
        private List<PlayerOnegaiModel> playerOnegaiModels;

        public PlayerOnegaiRepository(IOnegaiRepository onegaiRepository)
        {
            this.playerOnegaiModels = onegaiRepository.GetAll()
                .Select(model => new PlayerOnegaiModel(model.Id, model, OnegaiState.Lock.ToString()))
                .ToList();
        }

        public IEnumerable<PlayerOnegaiModel> GetAll()
        {
            return this.playerOnegaiModels
                .Select(model => new PlayerOnegaiModel(model.Id, model.OnegaiModel, model.OnegaiState.ToString()))
                .ToList();
        }

        public void Store(PlayerOnegaiModel playerOnegaiModel)
        {
            var index = playerOnegaiModels.FindIndex(model => model.Id == playerOnegaiModel.Id);
            Debug.Assert(index >= 0, "保存対象が見つかりませんでした。");
            playerOnegaiModels[index] = new PlayerOnegaiModel(playerOnegaiModel.Id, playerOnegaiModel.OnegaiModel, playerOnegaiModel.OnegaiState.ToString());
        }
    }
}
