using NL.EventContents;

namespace NL {
    public class EventContentsGenerator {
        public static IEventContents Generate(EventContentsModel eventContentsModel) {
            if (eventContentsModel.EventContentsType == EventContentsType.ForceConversation) {
                return new ForceConversation(eventContentsModel);
            }
            return new Invalid();
        }
    }
}
