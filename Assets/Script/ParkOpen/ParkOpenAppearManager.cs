using System;
using UnityEngine;

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
        public void AppearRandom(AppearCharacterModel appearCharacterModel)
        {
            var appearCharacterGenerator = new AppearCharacterGenerator(appearCharacterModel);
            var appearPositionModel = this.parkOpenPositionRepository.GerRandomPosition(ParkOpenPositionModel.PositionType.Appear);
            var disappearPositionModel = this.parkOpenPositionRepository.GerRandomPosition(ParkOpenPositionModel.PositionType.DisAppear);
            var movePath = new MovePath(
                appearPositionModel.Position + GetRandomOffsetPosition(),
                disappearPositionModel.Position + GetRandomOffsetPosition()
            );
            GameManager.Instance.AppearCharacterManager.EnqueueRegister(appearCharacterGenerator.GenerateParkOpen(movePath));
        }

        public Vector3 GetRandomOffsetPosition()
        {
            float range = 8;
            return new Vector3(UnityEngine.Random.Range(-range,range), 0, UnityEngine.Random.Range(-range,range));
        }

        public void AppearWave(ParkOpenWaveModel parkOpenWaveModel)
        {
            int loopCount = parkOpenWaveModel.AppearCount + UnityEngine.Random.Range(-parkOpenWaveModel.FluctuationCount,parkOpenWaveModel.FluctuationCount);
            for(int currentLoop = 0 ; currentLoop < loopCount ; currentLoop++ )
            {
                int appearCharacterIndex = UnityEngine.Random.Range(0, parkOpenWaveModel.AppearCharacterModels.Length - 1 );
                this.AppearRandom(parkOpenWaveModel.AppearCharacterModels[appearCharacterIndex]);
            }

        }
    }
}