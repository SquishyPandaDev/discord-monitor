using UnityEngine;

namespace DiscordMonitor {

  using WSReceive     = WS.Message.Receive;
  using WSReceiveKind = WS.Message.Receive.Kind;

  public class HandleBadMessage : MonoBehaviour {
    [SerializeField]
    private WSClientManager _wsClientManager = null;

    private void Update() {
      this._HandleNotJson();
      this._HandleBadMessage();
    }

    private void _HandleNotJson() {
      var filterResult = Library.Util.FilterMessage(
        this._wsClientManager,
        WSReceiveKind.NOT_JSON,
        out _
      );

      if(!filterResult) return;

      Debug.LogError(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .HandleBadMessage
              .NOT_JSON
          )
      );
    }

    private void _HandleBadMessage() {
      var filterResult = Library.Util.FilterMessage(
        this._wsClientManager,
        WSReceiveKind.BAD_MESSAGE,
        out WSReceive.Base rawMessage
      );

      if(!filterResult) return;

      var message = (WSReceive.BadMessage)rawMessage;

      Debug.LogError(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .HandleBadMessage
              .BAD_MESSAGE
          ) + message.metaInfo?.componentName
      );
    }
  }
}
