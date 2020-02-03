using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenCardView : MonoBehaviour
    {
        public enum AnimationTag
        {
            Initial,
            ToLarge,
            ToSmall
        }

        [SerializeField]
        private Text nameText = null;

        [SerializeField]
        private Text descriptionText = null;

        [SerializeField]
        private Image image = null;

        [SerializeField]
        private Button cardButton = null;

        [SerializeField]
        private Button cnacelButton = null;

        [SerializeField]
        private Button useButton = null;

        [SerializeField]
        private Animator animator = null;

        public TypeObservable<int> OnTouchCardObservable { get; private set; }
        public TypeObservable<int> OnCancelObservable { get; private set; }
        public TypeObservable<int> OnUseObservable { get; private set; }
        

        public void PlayAnimation(AnimationTag animationTag)
        {
            animator.SetTrigger(animationTag.ToString());
        }

        public void Initialize() {
            this.OnTouchCardObservable = new TypeObservable<int>();
            this.OnCancelObservable = new TypeObservable<int>();
            this.OnUseObservable = new TypeObservable<int>();
            this.cardButton.onClick.AddListener(()=>{
                this.OnTouchCardObservable.Execute(0);
            });
            this.cnacelButton.onClick.AddListener(()=>{
                this.OnCancelObservable.Execute(0);
            });
            this.useButton.onClick.AddListener(()=>{
                this.OnUseObservable.Execute(0);
            });
            SetPrepareButtonVisible(false);
        }

        public void UpdateView(string name, string description, Sprite sprite) {
            this.nameText.text = name;
            this.descriptionText.text = description;
            this.image.sprite = sprite;
        }

        public void SetPrepareButtonVisible(bool visible)
        {
            this.cnacelButton.gameObject.SetActive(visible);
            this.useButton.gameObject.SetActive(visible);
        }
    }   
}