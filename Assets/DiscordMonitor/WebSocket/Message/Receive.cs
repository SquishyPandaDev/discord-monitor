using System;
using Newtonsoft.Json;

namespace DiscordMonitor {
namespace WS {
namespace Message {

  public class Receive {
    public enum Kind {
      BAD_MESSAGE,
      NOT_JSON,
      STREAMED_CONNECTED_USER,
      UUID,
    }

    public static bool IsKind(
      Kind kind,
      Base message
    ) {
      return message.kind == kind;
    }

    public abstract class Base {
      public readonly Kind     kind;
      public readonly DateTime timeStamp;

      [JsonConstructor]
      public Base(
        string kind,
        string timeStamp
      ) {
        this.kind = (Kind)Enum.Parse(
          typeof(Kind),
          kind, true
        );

        this.timeStamp = DateTime.Parse(timeStamp);
      }
    }

    public class BadMessage : Base {
      public readonly string   messageRaw;
      public readonly MetaInfo metaInfo;

      [JsonConstructor]
      public BadMessage(
        string   kind,
        string   timeStamp,
        string   message,
        MetaInfo meta
      ) : base(kind, timeStamp) {
        this.messageRaw = message;
        this.metaInfo   = meta;
      }
    }

    public class NotJSON : Base {
      [JsonConstructor]
      public NotJSON(
        string kind,
        string timeStamp
      ) : base(kind, timeStamp) {}
    }

    public class StreamedConnectedUser : Base {
      public readonly string id;
      public readonly string name;
      public readonly string color;

      [JsonConstructor]
      public StreamedConnectedUser(
        string kind,
        string timeStamp,
        string id,
        string name,
        string color
      ) : base(kind, timeStamp) {
        this.id    = id;
        this.name  = name;
        this.color = color;
      }
    }

    public class UUID : Base {
      public readonly string uuid;

      [JsonConstructor]
      public UUID(
        string kind,
        string timeStamp,
        string uuid
      ) : base(kind, timeStamp) {
        this.uuid = uuid;
      }
    }
  }

}
}
}
