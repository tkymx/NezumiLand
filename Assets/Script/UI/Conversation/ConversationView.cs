using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NL {
    public class ConversationView : MonoBehaviour
    {
        private const float SpeakSpeed = 10.0f;

        [SerializeField]
        private Text speakerName = null;

        [SerializeField]
        private Text contents = null;

        [SerializeField]
        private Button nextNortificationAndButton = null;

        [SerializeField]
        private Button speakEndButton = null;

        private string allContents = "";
        private float elapsedTimeFromStart = 0;

        /// <summary>
        /// 会話が切り上げられたときのイベント
        /// </summary>
        private TypeObservable<int> onEndConversation = null;
        public TypeObservable<int> OnEndConversation => onEndConversation;

        public void Initialize() {
            this.speakEndButton.onClick.AddListener(()=>{
                // 最後まで表示される状態にする
                elapsedTimeFromStart = allContents.Length;
            });
            onEndConversation = new TypeObservable<int>();
            this.nextNortificationAndButton.onClick.AddListener(()=>{
                onEndConversation.Execute(0);
            });
        }

        // Update is called once per frame
        private void Update()
        {
            var currentDisplayCount = (int)(elapsedTimeFromStart*SpeakSpeed);
            var isOver = currentDisplayCount >= allContents.Length;
            if (isOver) {
                currentDisplayCount = allContents.Length;
            }
            contents.text = allContents.Substring(0,currentDisplayCount);

            // 制限文字を超えていたら
            if (isOver) {
                nextNortificationAndButton.gameObject.SetActive(true);
            }
            else {
                nextNortificationAndButton.gameObject.SetActive(false);
                elapsedTimeFromStart += GameManager.Instance.TimeManager.DeltaTimeWithoutPause();
            }
        }

        public void StartSpeak(string speakerName, string contents) {
            this.speakerName.text = speakerName;
            this.allContents = contents;
            this.elapsedTimeFromStart = 0;
        }
    }
}
