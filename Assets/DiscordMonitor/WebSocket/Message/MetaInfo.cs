using Newtonsoft.Json;

namespace DiscordMonitor {
namespace WS {
namespace Message {

  [JsonObject(MemberSerialization.OptIn)]
  public class MetaInfo {
    [JsonProperty]
    public readonly string componentName;

    [JsonConstructor]
    public MetaInfo(string componentName) {
      this.componentName = componentName;
    }
  }

}
}
}
