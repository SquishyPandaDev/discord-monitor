using UnityEngine;
using UnityEditor;

namespace DiscordMonitor {
  [CustomPropertyDrawer(typeof(ShowOnBooleanAttribute))]
  public class ShowOnBooleanDrawer : PropertyDrawer {
    public override void OnGUI(
      Rect               position,
      SerializedProperty property,
      GUIContent         label
    ) {
      var attribute = (ShowOnBooleanAttribute)this.attribute;

      var splitPropertyPath = property
        .propertyPath
        .Split('.');

      splitPropertyPath[splitPropertyPath.Length - 1] =
        attribute.booleanFieldName;

      var realPropertyPath = string.Join(
        ".",
        splitPropertyPath
      );

      var booleanField = property
        .serializedObject
        .FindProperty(realPropertyPath);

      if(booleanField == null) {
        this._ShowError(
          position,
          label,
          "Could not get boolean field for" +
            $" <{attribute.booleanFieldName}>"
        );

        return;
      }

      var isBooleanType = booleanField.propertyType
        == SerializedPropertyType.Boolean;

      if(!isBooleanType) {
        this._ShowError(
          position,
          label,
          $"<{attribute.booleanFieldName}> is not a boolean property"
        );

        return;
      }

      this._showField  = (attribute.invert)
        ? !booleanField.boolValue
        : booleanField.boolValue;

      if(this._showField) {
        EditorGUI.PropertyField(
          position,
          property,
          label
        );
      }
    }

    public override float GetPropertyHeight(
      SerializedProperty property,
      GUIContent label
    ) {
      return (this._showField)
        ? EditorGUI.GetPropertyHeight(property)
        : -(EditorGUIUtility.standardVerticalSpacing);
    }

    private bool _showField = true;

    private void _ShowError(
      Rect       position,
      GUIContent label,
      string     errorText
    ) {
      EditorGUI.LabelField(
        position,
        label,
        new GUIContent(errorText)
      );
    }
  }
}
