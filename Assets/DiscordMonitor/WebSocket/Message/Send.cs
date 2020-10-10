using Newtonsoft.Json;

namespace DiscordMonitor {
namespace WS {
namespace Message {

  [JsonConverter(typeof(CustomJsonConverter.Send))]
  public class Send {
    public string clientId { get; private set; }

    public readonly CommandList command;

    public readonly MetaInfo meta;

    public Send(CommandList command, MetaInfo meta = null) {
      this.command = command;
      this.meta    = meta;
    }

    public void StampClientId(string clientId) {
      this.clientId = clientId;
    }
  }

}
}
}
