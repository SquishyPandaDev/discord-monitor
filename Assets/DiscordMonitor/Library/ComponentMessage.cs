using System;
using System.ComponentModel;

namespace DiscordMonitor {
namespace Library {

  public static class ComponentMessage {
    public enum WebSocket {
      [Message("Connecting to: ")]
      CONNECTING_TO,

      [Message("Connection status: ")]
      CONNECTION_STATUS,

      [Message("Connection closed")]
      CONNECTION_CLOSED,

      [Message("Running reading loop")]
      READING_MESSAGES,

      [Message("Running sending loop")]
      SENDING_MESSAGES,
    }

    public enum WSClientManager {}

    public enum ReceiveQueueWatchDog {
      [Message("Message lifetime reached; droping: ")]
      DROPPED_MESSAGE,
    }

    public enum HandleBadMessage {
      [Message("Program sent invalid json")]
      NOT_JSON,

      [Message("The following component sent a malformed message: ")]
      BAD_MESSAGE
    }

    public static string GetMessage(Enum tag) {
      var componentTag = tag.GetType().Name;

      var message = Get
        .EnumAttribute<MessageAttribute>(tag)
        .message;

      return $"<{componentTag}> {message}";
    }

    [AttributeUsage(AttributeTargets.All)]
    private class MessageAttribute : Attribute {
      public string message { get; }

      public MessageAttribute(string message) {
        this.message = message;
      }
    }
  }

}
}
