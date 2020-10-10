using System;
using System.Linq;

namespace DiscordMonitor {
namespace Library {

  public static class Get {
    public static TAttribute EnumAttribute<TAttribute>(
      this Enum value
    ) {
      var type = value.GetType();
      var name = Enum.GetName(
        type,
        value
      );

      return type
        .GetField(name)
        .GetCustomAttributes(false)
        .OfType<TAttribute>()
        .SingleOrDefault();
    }
  }

}
}
