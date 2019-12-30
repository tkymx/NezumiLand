using UnityEngine;

namespace NL {
    public class EventContentsGenerator {
        public static IEventContents Generate(PlayerEventModel playerEventModel) {
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.Nope) {
                return new NL.EventContents.Nope(playerEventModel);
            }
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.ForceConversation) {
                return new NL.EventContents.ForceConversation(playerEventModel);
            }
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.AppearConversationCharacter) {
                return new NL.EventContents.AppearConversationCharacter(playerEventModel);
            }
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.AppearConversationCharacterNormal) {
                return new NL.EventContents.AppearConversationCharacterNormal(playerEventModel);
            }
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.ScreenImages) {
                return new NL.EventContents.ScreenImages(playerEventModel);
            }
            Debug.Assert(false,"Invalidなコンテンツが生成されました。");
            return new NL.EventContents.Invalid();
        }
    }
}
