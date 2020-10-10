
namespace DiscordMonitor {
namespace Library {

  using WSReceive = WS.Message.Receive;

  public static class Util {
    public static bool FilterMessage(
      WSClientManager  wsClientManager,
      WSReceive.Kind     wantedKind,
      out WSReceive.Base message
    ) {
      bool result = false;

      message = null;

      var peekResult = wsClientManager.TryReceivePeek(
        out WSReceive.Base tempMessage
      );

      if(peekResult) {
        if(tempMessage.kind == wantedKind) {
          result = wsClientManager.TryReceiveDequeue(out message);
        }
      }

      return result;
    }
  }
}
}
