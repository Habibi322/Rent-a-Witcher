using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(System.Enum), true)]
public class EnumSubmenuDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var enumNames = property.enumNames;
        for (var i = 0; i < enumNames.Length; i++)
        {
            enumNames[i] = enumNames[i].Replace("_", "/");
        }

        var currentValue = property.enumValueIndex;

        var newValue = EditorGUI.Popup(position, label.text, currentValue, enumNames);

        if (newValue != currentValue)
        {
            property.enumValueIndex = newValue;
        }
    }
}