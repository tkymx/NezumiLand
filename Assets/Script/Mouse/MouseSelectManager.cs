using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MouseSelectManager {
        private Mouse selectedMouse;
        public Mouse SelectedMouse => selectedMouse;
        public bool HasSelectedMouse => selectedMouse != null;

        public void SelectMouse (Mouse mouse) {
            this.selectedMouse = mouse;
        }

        public void RemoveSelect () {
            this.selectedMouse = null;
        }
    }
}