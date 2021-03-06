using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// イベント内容のインターフェース
    /// すぐ終わるのもあれば続くものもある。
    /// 内部でウィンドウを開いたり、場の内容が変わったりすることを想定している
    /// </summary>
    public interface IEventContents {
        EventContentsType EventContentsType { get; }
        PlayerEventModel TargetPlayerEventModel { get; }
        void OnEnter();
        void OnUpdate();
        void OnExit();
        bool IsAlive();
    }
}
