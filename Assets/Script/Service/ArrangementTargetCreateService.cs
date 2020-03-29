using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementTargetCreateService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public ArrangementTargetCreateService(IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        public PlayerArrangementTarget Execute (IPlayerArrangementTarget arrangementTarget) {
            var playerArrangementTargetModel = playerArrangementTargetRepository.Create(
                arrangementTarget.CenterPosition,
                arrangementTarget.Range,
                arrangementTarget.ArrangementPositions,
                arrangementTarget.ArrangementTargetState,
                null,
                null
            );
            return new PlayerArrangementTarget(playerArrangementTargetModel);
        }

        public PlayerArrangementTarget ExecuteForMoveIndicator (IPlayerArrangementTarget arrangementTarget) {
            var playerArrangementTargetModel = playerArrangementTargetRepository.Create(
                arrangementTarget.CenterPosition,
                arrangementTarget.Range,
                arrangementTarget.ArrangementPositions,
                ArrangementTargetState.MoveIndicator,
                null,
                null
            );
            return new PlayerArrangementTarget(playerArrangementTargetModel);
        }        
    }   
}