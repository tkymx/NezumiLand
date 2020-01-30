using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenCharacterCountPresenter : UiWindowPresenterBase
    {
        private IPlayerParkOpenRepository playerParkOpenRepository;

        [SerializeField]
        private ParkOpenCharacterCountView parkOpenCharacterCountView = null;

        public void Initialize(IPlayerParkOpenRepository playerParkOpenRepository) {
            this.playerParkOpenRepository = playerParkOpenRepository;
            this.Close();
        }

        public void UpdateByFrame() {
            this.parkOpenCharacterCountView.UpdateView(GameManager.Instance.AppearCharacterManager.ParkOpenCharacterCount.ToString());
        }
    }    
}