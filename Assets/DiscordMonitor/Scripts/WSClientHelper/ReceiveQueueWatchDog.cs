using UnityEngine;

namespace DiscordMonitor {

  public class ReceiveQueueWatchDog : MonoBehaviour {
    private uint _frameCount = 0;

    private WS.Message.Receive.Base _startFrameMessage;

    [Space]
    [SerializeField]
    private Avatar.Simulator _avatarSimulator = null;

    [Space]
    [SerializeField]
    private WSClientManager _wsClientManager = null;

    [Space]
    [Tooltip("How many frame(s) to keep messages in queue before dropping")]
    [Min(1)]
    [SerializeField]
    private uint _messageLifetime = 4;

    private void OnEnable() {
      if(this._avatarSimulator.DoSimulate) {
        this.enabled = false;
      }
    }

    private void Update() {
      this._frameCount++;

      // not at message lifetime
      if(this._frameCount < this._messageLifetime) return;

      this._frameCount = 0;

      // do we need to seed a start frame message
      if(this._startFrameMessage == null) {
        this._wsClientManager.TryReceivePeek(
          out this._startFrameMessage
        );

         return;
      }

      var peekResult = this._wsClientManager.TryReceivePeek(
        out WS.Message.Receive.Base currentMessage
      );

      if(peekResult) {
        // check time stamps to see if same message
        var isSameMessage =
          this._startFrameMessage.timeStamp
            == currentMessage.timeStamp;

        if(isSameMessage) {
          this._wsClientManager.TryReceiveDequeue(out _);

          Debug.LogWarning(
            Library
              .ComponentMessage
              .GetMessage(
                Library
                  .ComponentMessage
                  .ReceiveQueueWatchDog
                  .DROPPED_MESSAGE
              ) + currentMessage.kind.ToString()
          );
        }

        this._startFrameMessage = currentMessage;
      }
    }
  }

}
