using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL
{
    public class PreMono
    {
        private Mouse mouse;
        private GameObject makingPrefab;
        private MonoInfo mono;

        private GameObject makingInstane;

        public PreMono(Mouse mouse, GameObject makingPrefab, MonoInfo mono)
        {
            this.mouse = mouse;
            this.makingPrefab = makingPrefab;
            this.mono = mono;
        }

        public void StartMaking(IArrangementTarget arrangementTarget)
        {
            this.makingInstane = Object.AppearToFloor(makingPrefab, mouse.transform.parent.gameObject, arrangementTarget.CenterPosition);
        }

        public void FinishMaking(IArrangementTarget arrangementTarget)
        {
            Debug.Assert(!arrangementTarget.HasMonoViewModel, "モノがセットされています。");

            Object.DisAppear(this.makingInstane);
            arrangementTarget.MonoViewModel = GameManager.Instance.MonoManager.CreateMono(mono, arrangementTarget.CenterPosition);

            // 隣接するターゲットを取得
            var targetArrangementTargets = GameManager.Instance.ArrangementManager
                .GetNearArrangement(arrangementTarget);
            targetArrangementTargets.Add(arrangementTarget);

            // 隣接オブジェクトに対してNearの判断を行う
            var playerOnegaiRepository = PlayerOnegaiRepository.GetRepository();
            var onegaiMediater = new OnegaiMediater(playerOnegaiRepository);
            foreach (var targetArrangementTarget in targetArrangementTargets)
            {
                var targetMonoInfoId = targetArrangementTarget.MonoInfo.Id;
                var nearMonoInfoIds = GameManager.Instance.ArrangementManager
                    .GetNearArrangement(targetArrangementTarget)
                    .Select(nearArrangementTarget => nearArrangementTarget.MonoInfo.Id)
                    .ToList();
                onegaiMediater.Mediate(new Near(nearMonoInfoIds), targetMonoInfoId);                
            }
        }
    }
}
