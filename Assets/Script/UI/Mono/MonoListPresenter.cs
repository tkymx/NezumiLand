using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoListPresenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject cellPrefab = null;

        [SerializeField]
        private GameObject monoListRoot = null;

        [SerializeField]
        private GameObject cellViewRoot = null;

        private IMonoInfoRepository monoInfoRepository = null;
        private Dictionary<MonoInfo, MonoListCellView> displayMonoCellDictionary;

        public void Initialize()
        {
            this.monoInfoRepository = new MonoInfoRepository(ContextMap.DefaultMap);
            this.displayMonoCellDictionary = new Dictionary<MonoInfo, MonoListCellView>();
            this.ReLoad();
            this.Close();
        }

        public void ReLoad()
        {
            // 寄贈の要素を消去
            foreach (Transform child in cellViewRoot.transform)
            {
                Object.DisAppear(child.gameObject);
            }

            this.displayMonoCellDictionary.Clear();

            // 要素の追加を行う
            foreach (var monoInfo in monoInfoRepository.GetAll())
            {
                var instance = Object.Appear2D(cellPrefab, cellViewRoot, Vector2.zero);
                var cellView = instance.GetComponent<MonoListCellView>();
                cellView.Initialize();
                cellView.UpdateCell(monoInfo.Name, monoInfo.MakingFee);
                cellView.OnClick
                    .Subscribe(_ =>
                    {
                        GameManager.Instance.MonoSelectManager.SelectMonoInfo(monoInfo);
                    });
                this.displayMonoCellDictionary.Add(monoInfo, cellView);
            }
        }

        public void Update()
        {
            var currentCurrency = GameManager.Instance.Wallet.CurrentCurrency;

            foreach (var pair in this.displayMonoCellDictionary)
            {
                var monoInfo = pair.Key;
                var cellView = pair.Value;

                if (!ArrangementResourceHelper.IsConsume(monoInfo.ArrangementResourceAmount))
                {
                    cellView.DiasbleForLowFee();
                }
                else
                {
                    cellView.Enable();
                }
            }
        }

        public void Show()
        {
            this.monoListRoot.gameObject.SetActive(true);
            this.ReLoad();
        }

        public void Close()
        {
            this.monoListRoot.gameObject.SetActive(false);
        }
    }
}
