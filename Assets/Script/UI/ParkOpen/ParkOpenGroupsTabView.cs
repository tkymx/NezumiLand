using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenGroupsTabView : MonoBehaviour
    {
        [SerializeField]
        private List<Button> tabButtons = null;

        [SerializeField]
        private List<ParkOpenGroupsType> displayParkOpenGroupsTypes = null;

        [SerializeField]
        private Button back = null;

        public TypeObservable<ParkOpenGroupsType> OnClickTypeObservable { get; private set; }
        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {

            Debug.Assert (this.tabButtons.Count > 0, "タブの項目がありません");
            Debug.Assert (this.tabButtons.Count == this.displayParkOpenGroupsTypes.Count, "ボタンとタイプの個数が異なります。");

            this.OnBackObservable = new TypeObservable<int>();
            this.OnClickTypeObservable = new TypeObservable<ParkOpenGroupsType>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
            for (int index = 0; index < displayParkOpenGroupsTypes.Count; index++ )
            {
                var button = this.tabButtons[index];
                var type = this.displayParkOpenGroupsTypes[index];
                button.onClick.AddListener(() => {
                    this.OnClickTypeObservable.Execute(type);
                });
            }
        }

        public void UpdateButtonEnabled(ParkOpenGroupsType selectType) {
            for (int index = 0; index < displayParkOpenGroupsTypes.Count; index++ )
            {
                var button = this.tabButtons[index];
                var type = this.displayParkOpenGroupsTypes[index];
                button.interactable = type != selectType;
            }
        }
    }   
}