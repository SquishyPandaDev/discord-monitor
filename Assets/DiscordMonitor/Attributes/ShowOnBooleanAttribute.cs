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
  public class ShowOnBooleanAttribute : PropertyAttribute {
    public readonly string booleanFieldName;
    public readonly bool   invert;

    public ShowOnBooleanAttribute(
      string conditionFieldName,
      bool   invert = false
    ) {
      this.booleanFieldName = conditionFieldName;
      this.invert           = invert;
    }
  }

}
