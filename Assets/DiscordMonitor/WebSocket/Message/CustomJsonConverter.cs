using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordMonitor {
namespace WS {
namespace Message {
namespace CustomJsonConverter {

  public class Receive : JsonConverter {
    public override bool CanConvert(Type objectType) {
      return objectType == typeof(Message.Receive.Base);
    }

    public override object ReadJson(
      JsonReader     reader,
      Type           objectType,
      object         existingValue,
      JsonSerializer serializer
    ) {
      JObject jObj = JObject.Load(reader);

      var kind = jObj["kind"].Value<string>();

      var kindAsEnum =
        (Message.Receive.Kind)Enum.Parse(
          typeof(Message.Receive.Kind),
          kind
        );

      switch(kindAsEnum) {
        case Message.Receive.Kind.BAD_MESSAGE:
          return jObj.ToObject<Message.Receive.BadMessage>(serializer);

        case Message.Receive.Kind.NOT_JSON:
          return jObj.ToObject<Message.Receive.NotJSON>(serializer);

        case Message.Receive.Kind.STREAMED_CONNECTED_USER:
          return jObj.ToObject<Message.Receive.StreamedConnectedUser>(
            serializer
          );

        case Message.Receive.Kind.UUID:
          return jObj.ToObject<Message.Receive.UUID>(serializer);

        default:
          return null;
      }
    }

    public override bool CanWrite => false;

    public override void WriteJson(
      JsonWriter     writer,
      object         value,
      JsonSerializer serializer
    ) {
      throw new NotImplementedException();
    }
  }

  public class Send : JsonConverter {
    public override void WriteJson(
      JsonWriter writer,
      object value,
      JsonSerializer serializer
    ) {
      var send = value as Message.Send;

      writer.WriteStartObject();

      writer.WritePropertyName(nameof(send.clientId));
      serializer.Serialize(writer, send.clientId);

      writer.WritePropertyName(nameof(send.command));
      serializer.Serialize(
        writer,
        Enum.GetName(
          typeof(Message.CommandList),
          send.command
        )
      );

      if(send.meta != null) {
        writer.WritePropertyName(nameof(send.meta));
        serializer.Serialize(writer, send.meta);
      }

      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
      return typeof(Message.Send).IsAssignableFrom(objectType);
    }
  }

}
}
}
}
