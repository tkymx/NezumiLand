using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterManager
    {
        private GameObject root = null;
        public GameObject Root => root;

        private List<AppearCharacterViewModel> appearCharacterViewModels = null;
        private Queue<AppearCharacterViewModel> reservedRemovableModelQueue = null;

        public AppearCharacterManager(GameObject root)
        {
            this.root = root;
            this.appearCharacterViewModels = new List<AppearCharacterViewModel>();
            this.reservedRemovableModelQueue = new Queue<AppearCharacterViewModel>();
        }

        public void UpdateByFrame()
        {
            foreach (var appearCharacterViewModel in appearCharacterViewModels)
            {
                appearCharacterViewModel.UpdateByFrame();                
            }
            while(this.reservedRemovableModelQueue.Count > 0) {
                var reservedRemovableModel = this.reservedRemovableModelQueue.Dequeue();
                Remove(reservedRemovableModel);
            }
        }

        public void Register(AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModels.Add(appearCharacterViewModel);
        }

        public void EnqueueRemove (AppearCharacterViewModel appearCharacterViewModel) {
            this.reservedRemovableModelQueue.Enqueue(appearCharacterViewModel);
        }

        private void Remove (AppearCharacterViewModel appearCharacterViewModel) {
            if (this.appearCharacterViewModels.IndexOf(appearCharacterViewModel) < 0) {
                return;
            }
            this.appearCharacterViewModels.Remove(appearCharacterViewModel);
        }
    }
}
