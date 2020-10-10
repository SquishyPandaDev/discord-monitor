using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;
using Newtonsoft.Json;

namespace DiscordMonitor {
namespace WS {

  public class Client {
    #region Public

    public Client(
      string serverURL,
      int    receiveBufferKBSize
    ) {
      this._serverUri = new Uri(serverURL);

      this._receiveBufferSize = (receiveBufferKBSize >= 1)
        ? receiveBufferKBSize * 1024
        : 1024;

      this._UUID = null;

      this._receiveThread      = new Thread(this._DoReceive);
      this._receiveThread.Name =
        $"{typeof(Library.ComponentMessage.WebSocket).Name} Receive";

      this._sendThread      = new Thread(this._DoSend);
      this._sendThread.Name =
        $"{typeof(Library.ComponentMessage.WebSocket).Name} Send";
    }

    public bool NeedToClose {
      get {
        lock(_sync) {
          return this._needToClose_unsafe;
        }
      }

      private set {
        lock(_sync) {
          this._needToClose_unsafe = value;
        }
      }
    }

    public async Task ConnectAsync() {
      if(this._IsConnectionOpen()) return;

      if(!this._receiveThread.IsAlive) {
        this._receiveThread.Start();
      }

      if(!this._sendThread.IsAlive) {
        this._sendThread.Start();
      }

      this._CancellationToken = new CancellationTokenSource();

      Debug.Log(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .WebSocket
              .CONNECTING_TO
          ) + this._serverUri
      );

      await this._webSocket.ConnectAsync(
        this._serverUri,
        CancellationToken.None
      );

      Debug.Log(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .WebSocket
              .CONNECTION_STATUS
          ) + this._webSocket.State
      );
    }

    public async Task DisconnectAsync() {
      if (!this._IsConnectionOpen()) return;

      this._CancellationToken.Cancel();

      if(this._webSocket.State != WebSocketState.Aborted) {
        await this._webSocket.CloseAsync(
          WebSocketCloseStatus.NormalClosure,
          "Closing",
          CancellationToken.None
        );
      }

      while (this._receiveQueue.TryDequeue(out _)) ;

      while (this._sendQueue.Count > 0)
      {
        this._sendQueue.TryTake(out _);
      }

      this._UUID = null;

      Debug.Log(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .WebSocket
              .CONNECTION_CLOSED
          )
      );
    }

    #region Queue Access
      public bool TryReceivePeek(out Message.Receive.Base message) =>
        this._receiveQueue.TryPeek(out message);

      public bool TryReceiveDequeue(out Message.Receive.Base message) =>
        this._receiveQueue.TryDequeue(out message);

      public void QueueSend(Message.Send message) =>
        this._sendQueue.Add(message);
    #endregion

    #endregion

    private const int DISCONNECT_TIMEOUT_MS = 500;

    private static object _sync = new object();

    private bool _needToClose_unsafe = false;

    private Uri _serverUri;
    private int _receiveBufferSize;

    private string _uuid_unsafe = null;

    private readonly ConcurrentQueue<Message.Receive.Base> _receiveQueue
      = new ConcurrentQueue<Message.Receive.Base>();

    private readonly BlockingCollection<Message.Send> _sendQueue
      = new BlockingCollection<Message.Send>();

    private readonly Thread _receiveThread;
    private readonly Thread _sendThread;

    private ClientWebSocket _webSocket = new ClientWebSocket();

    private CancellationTokenSource _cancellationToken_unsafe =
      new CancellationTokenSource();

    private CancellationTokenSource _CancellationToken {
      get {
        lock(_sync) {
          return this._cancellationToken_unsafe;
        }
      }

      set {
        lock(_sync) {
          this._cancellationToken_unsafe = value;
        }
      }
    }

    private string _UUID {
      get {
        lock(_sync) {
          return this._uuid_unsafe;
        }
      }

      set {
        lock(_sync) {
          this._uuid_unsafe = value;
        }
      }
    }

    private bool _IsConnectionOpen() {
      lock(_sync) {
        return this._webSocket.State == WebSocketState.Open
          && !this._CancellationToken.IsCancellationRequested;
      }
    }

    private async void _DoReceive() {
      Debug.Log(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .WebSocket
              .READING_MESSAGES
          )
      );

      while(!this._CancellationToken.IsCancellationRequested) {
        try {
          var message = (this._IsConnectionOpen())
            ? await this._Receive()
            : null;

          if(message != null && message.Length > 0) {
            var deserializedMessage =
              JsonConvert.DeserializeObject<Message.Receive.Base>(
                message,
                new Message.CustomJsonConverter.Receive()
              );

            var isUuidMessage =
              Message.Receive.IsKind(
                Message.Receive.Kind.UUID,
                deserializedMessage
              );

            if(isUuidMessage) {
              this._UUID =
                ((Message.Receive.UUID)deserializedMessage).uuid;
            } else {
              this._receiveQueue.Enqueue(deserializedMessage);
            }
          } else {
            Task.Delay(50).Wait();
          }
        } catch(OperationCanceledException) {
        } catch(Exception error) {
          Debug.LogError(error.Message);
        }
      }
    }

    private async void _DoSend() {
      Debug.Log(
        Library
          .ComponentMessage
          .GetMessage(
            Library
              .ComponentMessage
              .WebSocket
              .SENDING_MESSAGES
          )
      );

      while(true) {
        var shouldSend = this._IsConnectionOpen()
          && this._UUID != null
          && this._sendQueue.Count > 0;

        if(shouldSend) {
          var message = this._sendQueue.Take();

          message.StampClientId(this._UUID);

          try {
            await this._Send(message);
          } catch(OperationCanceledException) {}
        } else {
          Task.Delay(50).Wait();
        }
      }
    }

    private async Task<string> _Receive() {
      var cancellationToken = this._CancellationToken.Token;

      var memoryStream = new MemoryStream();
      var result       = "";

      WebSocketReceiveResult chunkResult = null;

      #region Receive Loop

      do {
        var messageBuffer = WebSocket.CreateClientBuffer(
          this._receiveBufferSize,
          16
        );

        chunkResult = await this._webSocket.ReceiveAsync(
          messageBuffer,
          cancellationToken
        );

        var needToClose =
          this._webSocket.State == WebSocketState.CloseReceived
          && chunkResult.MessageType == WebSocketMessageType.Close;

        if(needToClose) {
          this.NeedToClose = true;
          return result;
        }

        memoryStream.Write(
          messageBuffer.Array,
          messageBuffer.Offset,
          chunkResult.Count
        );
      } while(!chunkResult.EndOfMessage);

      #endregion

      memoryStream.Seek(0, SeekOrigin.Begin);

      if(chunkResult.MessageType == WebSocketMessageType.Text) {
        result = Encoding
          .UTF8
          .GetString(memoryStream.ToArray());
      }

      return result;
    }

    private async Task _Send(Message.Send message) {
      var serializedMessage = JsonConvert.SerializeObject(message);

      var buffer = Encoding
        .UTF8
        .GetBytes(serializedMessage);

      var sendBuffer = new ArraySegment<byte>(buffer);

      await this._webSocket.SendAsync(
        sendBuffer,
        WebSocketMessageType.Text,
        true,
        CancellationToken.None
      );
    }
  }

}
}
