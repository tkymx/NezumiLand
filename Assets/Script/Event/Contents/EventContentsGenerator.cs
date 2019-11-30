using NL.EventContents;

namespace NL {
    public class EventContentsGenerator {
        public static IEventContents Generate(PlayerEventModel playerEventModel) {
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.Nope) {
                return new Nope(playerEventModel);
            }
            if (playerEventModel.EventModel.EventContentsModel.EventContentsType == EventContentsType.ForceConversation) {
                return new ForceConversation(playerEventModel);
            }
            return new Invalid();
        }
    }
}
