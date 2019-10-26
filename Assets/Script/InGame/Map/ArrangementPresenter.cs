﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementPresenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject arrangementPrefab = null;

        [SerializeField]
        private GameObject selectedArrangementPrefab = null;

        // のちのち view として作れれば良い
        private List<ArrangementView> arrangementViews;

        private void Start()
        {
            this.arrangementViews = new List<ArrangementView>();
        }

        public void ReLoad()
        {
            foreach (var arrangementView in this.arrangementViews)
            {
                GameObject.Destroy(arrangementView.gameObject);
            }

            this.arrangementViews.Clear();

            GameManager.Instance.ArrangementManager.ArrangementTargetStore.ForEach(arrangementTarget =>
            {
                if (GameManager.Instance.ArrangementManager.CheckIsSelect(arrangementTarget))
                {
                    this.appearArrangement(arrangementTarget, selectedArrangementPrefab);

                }
                else
                {
                    this.appearArrangement(arrangementTarget, arrangementPrefab);
                }
            });
        }

        private void appearArrangement(IArrangementTarget arrangementTarget, GameObject prefab)
        {
            arrangementTarget.ArrangementPositions.ForEach(arrangementPosition =>
            {
                var instance = Object.Appear(prefab, gameObject, new Vector3(
                    arrangementPosition.x * ArrangementAnnotater.ArrangementWidth,
                    0,
                    arrangementPosition.z * ArrangementAnnotater.ArrangementHeight
                ));

                Debug.Assert(instance != null, "Arrangement が生成されていません");
                Debug.Assert(instance.GetComponent<ArrangementView>() != null, "Arrangement の ArrangementView が設定されておりません。");

                var arrangementView = instance.GetComponent<ArrangementView>();

                // 選択時の挙動を追加
                arrangementView.OnSelect.Subscribe(new TypeObserver<int>(_ =>
                {
                    GameManager.Instance.ArrangementManager.Select(arrangementTarget);
                    this.ReLoad();
                }));

                this.arrangementViews.Add(arrangementView);
            });
        }
    }
}
