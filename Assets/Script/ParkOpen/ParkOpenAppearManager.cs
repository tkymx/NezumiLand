using System;

namespace NL
{
    public class ParkOpenAppearManager {
        
        private IParkOpenPositionRepository parkOpenPositionRepository;
        private IAppearCharacterRepository appearCharacterRepository;

        public ParkOpenAppearManager(IParkOpenPositionRepository parkOpenPositionRepository, IAppearCharacterRepository appearCharacterRepository)
        {
            this.parkOpenPositionRepository = parkOpenPositionRepository;
            this.appearCharacterRepository = appearCharacterRepository;            
        }       

        /// <summary>
        /// ランダム位置に適当なキャラクタを召喚
        /// </summary>
        public void AppearRandom()
        {
            var positionModel = this.parkOpenPositionRepository.GerRandomPosition(ParkOpenPositionModel.PositionType.Appear);
            var appearCharacterModel = this.appearCharacterRepository.Get(1);   // 外から指定したい
            var appearCharacterGenerator = new AppearCharacterGenerator(appearCharacterModel);
            GameManager.Instance.AppearCharacterManager.EnqueueRegister(appearCharacterGenerator.GenerateParkOpen(positionModel.Position));
        }
    }
}