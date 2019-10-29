using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoManager
    {
        GameObject root;
        List<MonoViewModel> monoViewModels;

        public MonoManager(GameObject root)
        {
            this.root = root;
            this.monoViewModels = new List<MonoViewModel>();
        }

        public void UpdateByFrame()
        {
            foreach (var monoViewModel in this.monoViewModels)
            {
                monoViewModel.UpdateByFrame();
            }
        }

        public MonoViewModel CreateMono(MonoInfo monoInfo, Vector3 position)
        {
            var instance = Object.AppearToFloor(monoInfo.monoPrefab, root, position);
            var monoView = instance.GetComponent<MonoView>();
            Debug.Assert(monoView != null, "monoViewではありません");

            var monoViewModel = new MonoViewModel(monoView);
            monoViewModels.Add(monoViewModel);

            return monoViewModel;
        }

        public void RemoveMono(MonoViewModel monoViewModel)
        {
            Debug.Assert(this.monoViewModels.Contains(monoViewModel), "消去予定のものが存在しません");
            Object.DisAppear(monoViewModel.MonoView.gameObject);
            monoViewModels.Remove(monoViewModel);
        }
    }
}