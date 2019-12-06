using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public class AppearCharacterManager
    {
        private GameObject root = null;
        public GameObject Root => root;

        private List<AppearCharacterViewModel> appearCharacterViewModels = null;
        private List<AppearCharacterViewModel> reservedRegisterModels = null;
        private List<AppearCharacterViewModel> reservedRemovableModels = null;

        public AppearCharacterManager(GameObject root)
        {
            this.root = root;
            this.appearCharacterViewModels = new List<AppearCharacterViewModel>();
            this.reservedRegisterModels = new List<AppearCharacterViewModel>();
            this.reservedRemovableModels = new List<AppearCharacterViewModel>();
        }

        public void UpdateByFrame()
        {
            // 追加
            foreach (var reservedRegisterModel in this.reservedRegisterModels)
            {
                this.Register(reservedRegisterModel);                
            }
            this.reservedRegisterModels.Clear();

            // 更新      
            foreach (var appearCharacterViewModel in appearCharacterViewModels)
            {
                appearCharacterViewModel.UpdateByFrame();                
            }

            // 消去
            foreach (var reservedRemovableModel in this.reservedRemovableModels)
            {
                this.Remove(reservedRemovableModel);
            }
            this.reservedRemovableModels.Clear();
        }

        public void EnqueueRegister (AppearCharacterViewModel appearCharacterViewModel) {
            this.reservedRegisterModels.Add(appearCharacterViewModel);
        }

        private void Register(AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModels.Add(appearCharacterViewModel);
        }

        private void EnqueueRemove (AppearCharacterViewModel appearCharacterViewModel) {
            this.reservedRemovableModels.Add(appearCharacterViewModel);
        }

        private void Remove (AppearCharacterViewModel appearCharacterViewModel) {
            if (this.appearCharacterViewModels.IndexOf(appearCharacterViewModel) < 0) {
                return;
            }
            appearCharacterViewModel.Dispose();
            this.appearCharacterViewModels.Remove(appearCharacterViewModel);
        }

        public void RemoveAll() {
            foreach (var reservedRegisterModel in this.appearCharacterViewModels) {
                this.EnqueueRemove(reservedRegisterModel);
            }                        
        }
    }
}
