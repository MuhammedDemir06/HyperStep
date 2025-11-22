using UnityEditor;
using UnityEngine;
using IronTools.Attributes;

[CustomPropertyDrawer(typeof(ShowTitleAttribute))]
public class ShowTitleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowTitleAttribute titleAttr = (ShowTitleAttribute)attribute;

        GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState { textColor = EditorColorPalette.Resolve(titleAttr.color) }
        };

        Rect titleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(titleRect, titleAttr.title, style);

        Rect fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 2;
    }
}
