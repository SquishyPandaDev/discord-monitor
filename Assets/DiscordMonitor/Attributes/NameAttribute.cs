using System;
using UnityEngine;

namespace DiscordMonitor {

  [
    AttributeUsage(
      AttributeTargets.Field,
      Inherited = true,
      AllowMultiple = false
    )
  ]
  public class NameAttribute : PropertyAttribute {
    public readonly string name;

    public NameAttribute(string name) {
      this.name = name;
    }
  }

}
