using UnityEngine;

namespace DiscordMonitor {

  public class WSClientManager : MonoBehaviour {
    public bool TryReceivePeek(out WS.Message.Receive.Base message) =>
      this._webSocketClient.TryReceivePeek(out message);

    public bool TryReceiveDequeue(out WS.Message.Receive.Base message) =>
      this._webSocketClient.TryReceiveDequeue(out message);

    private const string _COMPONENT_NAME =
      nameof(Library.ComponentMessage.WSClientManager);

    [SerializeField]
    [Space(10)]
    private Host _host = new Host();

    [SerializeField]
    [Space(10)]
    [Name("Receive Buffer Size in KB")]
    [Min(1)]
    private int _receiveBufferSize = 1;

    private string _server => (
        this._host.useLocal
          || (this._host.name.Length == 0)
    )
      ? "localhost"
      : this._host.name;

    private string _serverUri => $"ws://{this._server}:{this._host.port}";

    private void OnEnable() {
      if(this._webSocketClient == null) {
        this._webSocketClient = new WS.Client(
          this._serverUri,
          this._receiveBufferSize
        );
      }

      _ = _webSocketClient.ConnectAsync();

      this._webSocketClient.QueueSend(
        new WS.Message.Send(
          WS.Message.CommandList.READY_FOR_EVENTS,
          new WS.Message.MetaInfo(_COMPONENT_NAME)
        )
      );

      this._webSocketClient.QueueSend(
        new WS.Message.Send(
          WS.Message.CommandList.STREAM_CONNECTED_USERS,
          new WS.Message.MetaInfo(_COMPONENT_NAME)
        )
      );
    }

    private void Update() {
      if(this._webSocketClient.NeedToClose) {
        _ = this._webSocketClient.DisconnectAsync();
      }
    }

    private void OnDisable() {
      _ = this._webSocketClient.DisconnectAsync();
    }

    public WS.Client _webSocketClient = null;

    [System.Serializable]
    private class Host {
      public bool useLocal {
        get { return this._useLocal; }
      }

      public string name {
        get { return this._name; }
      }

      public uint port {
        get { return this._port; }
      }

      [SerializeField]
      private bool _useLocal = true;

      [SerializeField]
      [
        ShowOnBoolean(
          "_useLocal",
          true
        )
      ]
      private string _name = "";

      [SerializeField]
      private uint _port = 9000;
    }
  }

}
