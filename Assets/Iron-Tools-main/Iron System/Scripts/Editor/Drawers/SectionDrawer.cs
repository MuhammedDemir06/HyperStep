using UnityEditor;
using UnityEngine;
using IronTools.Attributes;    

[CustomPropertyDrawer(typeof(SectionAttribute))]
public class SectionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SectionAttribute section = (SectionAttribute)attribute;
        Color bgColor = EditorColorPalette.Resolve(section.ColorEnum);

        Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight + 4);
        EditorGUI.DrawRect(headerRect, bgColor);

        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState { textColor = Color.white }
        };

        Rect propertyRect = new Rect(position.x, headerRect.y + headerRect.height + 2, position.width, EditorGUI.GetPropertyHeight(property, label, true));
        EditorGUI.PropertyField(propertyRect, property, label, true);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float headerHeight = EditorGUIUtility.singleLineHeight + 4;
        float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
        return headerHeight + propertyHeight + 2;
    }
}
