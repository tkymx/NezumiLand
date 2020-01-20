using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public class EarnCurrencyManager
    {
        private GameObject root = null;
        public GameObject Root => root;

        private List<EarnCurrencyViewModel> earnCurrencyViewModels = null;
        private List<EarnCurrencyViewModel> additionalEarnCurrencyViewModels = null;
        private List<EarnCurrencyViewModel> removableEarnCurrencyViewModels = null;

        private EarnCurrencyCreateService earnCurrencyCreateService = null;
        private EarnCurrencyRemoveService earnCurrencyRemoveService = null;
        private EarnCurrencyAddService earnCurrencyAddService = null;

        public EarnCurrencyManager(GameObject root, IPlayerEarnCurrencyRepository playerEarnCurrencyRepository)
        {
            this.root = root;

            this.earnCurrencyViewModels = new List<EarnCurrencyViewModel>();
            this.additionalEarnCurrencyViewModels = new List<EarnCurrencyViewModel>();
            this.removableEarnCurrencyViewModels = new List<EarnCurrencyViewModel>();

            // service の初期化を行う
            this.earnCurrencyCreateService = new EarnCurrencyCreateService(playerEarnCurrencyRepository);
            this.earnCurrencyRemoveService = new EarnCurrencyRemoveService(playerEarnCurrencyRepository);
            this.earnCurrencyAddService = new EarnCurrencyAddService(playerEarnCurrencyRepository);
        }

        public void UpdateByFrame()
        {
            // 追加
            foreach (var additionalEarnCurrencyViewModel in this.additionalEarnCurrencyViewModels)
            {
                this.earnCurrencyViewModels.Add(additionalEarnCurrencyViewModel);                
            }
            this.additionalEarnCurrencyViewModels.Clear();

            // 更新      
            foreach (var earnCurrencyViewModel in earnCurrencyViewModels)
            {
                earnCurrencyViewModel.UpdateByFrame();                
            }

            // 消去
            foreach (var removableEarnCurrencyViewModel in this.removableEarnCurrencyViewModels)
            {
                this.Remove(removableEarnCurrencyViewModel);
            }
            this.removableEarnCurrencyViewModels.Clear();
        }

        public void CreateOrAdd (PlayerArrangementTargetModel playerArrangementTargetModel, Currency earnCurrency)
        {
            var resultModel = Contain(playerArrangementTargetModel);
            if (resultModel != null) {
                this.AddEarnCurrency(resultModel.playerEarnCurrencyModel, earnCurrency);
            }
            else {
                this.Create(playerArrangementTargetModel, earnCurrency);
            }
        }

        private EarnCurrencyViewModel Contain (PlayerArrangementTargetModel playerArrangementTargetModel)
        {
            var resultModel = additionalEarnCurrencyViewModels.Find(model => model.playerEarnCurrencyModel.PlayerArrangementTargetModel.Equals(playerArrangementTargetModel));
            if (resultModel != null) {
                Debug.Assert(false, "additionalEarnCurrencyViewModelsにある要素が指定されました。");
                return resultModel;
            }

            resultModel = earnCurrencyViewModels.Find(model => model.playerEarnCurrencyModel.PlayerArrangementTargetModel.Equals(playerArrangementTargetModel));
            if (resultModel != null) {
                return resultModel;
            }

            return null;
        }

        /// <summary>
        /// PlayerEarnCurrencyModel から 生成する場合
        /// </summary>
        /// <param name="earnCurrencyView"></param>
        /// <param name="playerEarnCurrencyModel"></param>
        public void CreateFromEarnCurrencyModel (PlayerEarnCurrencyModel playerEarnCurrencyModel) 
        {
            string modelName = "Coin";
            Vector3 offsetFromTarget = new Vector3(0,10,0);

            var modelPrefab = ResourceLoader.LoadModel(modelName);
            var earnCurrencyInstance = Object.Appear(modelPrefab, root , playerEarnCurrencyModel.PlayerArrangementTargetModel.CenterPosition + offsetFromTarget);
            var earnCurrencyView = earnCurrencyInstance.GetComponentInChildren<EarnCurrencyView>();
            var generatedearnCurrencyViewModel = new EarnCurrencyViewModel(
                earnCurrencyView,
                playerEarnCurrencyModel
            );
            this.additionalEarnCurrencyViewModels.Add(generatedearnCurrencyViewModel);
        }

        private void Create (PlayerArrangementTargetModel playerArrangementTargetModel, Currency earnCurrency) 
        {
            var playerEarnCurrencyModel = this.earnCurrencyCreateService.Execute(playerArrangementTargetModel, earnCurrency);
            this.CreateFromEarnCurrencyModel(playerEarnCurrencyModel);
        }

        public void EnqueueRemove(EarnCurrencyViewModel earnCurrencyViewModel)
        {
            this.removableEarnCurrencyViewModels.Add(earnCurrencyViewModel);
        }

        private void Remove(EarnCurrencyViewModel earnCurrencyViewModel)
        {
            earnCurrencyViewModel.Dispose();
            earnCurrencyViewModels.Remove(earnCurrencyViewModel);
            this.earnCurrencyRemoveService.Execute(earnCurrencyViewModel.playerEarnCurrencyModel);
        }

        private void AddEarnCurrency (PlayerEarnCurrencyModel playerEarnCurrencyModel, Currency additionalEarnCurrency) 
        {
            this.earnCurrencyAddService.Execute(playerEarnCurrencyModel, additionalEarnCurrency);            
        }
    }
}
