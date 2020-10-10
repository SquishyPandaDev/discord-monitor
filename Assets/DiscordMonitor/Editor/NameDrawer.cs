using UnityEngine;
using UnityEditor;

namespace DiscordMonitor {
  [CustomPropertyDrawer(typeof(NameAttribute))]
  public class NameDrawer : PropertyDrawer {
    public override void OnGUI(
      Rect               position,
      SerializedProperty property,
      GUIContent         label
    ) {
      EditorGUI.PropertyField(
        position,
        property,
        new GUIContent(
          (this.attribute as NameAttribute).name
        )
      );
    }
  }
}
