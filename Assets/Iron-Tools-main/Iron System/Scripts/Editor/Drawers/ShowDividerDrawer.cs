using IronTools.Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowDividerAttribute))]
public class ShowDividerDrawer : DecoratorDrawer
{
    private const float LineHeight = 2f;
    private const float SpaceBefore = 10f;
    private const float SpaceAfter = 5f;
    private const float LabelHeight = 18f;

    public override float GetHeight()
    {
        ShowDividerAttribute attr = (ShowDividerAttribute)attribute;
        return SpaceBefore + LineHeight + (string.IsNullOrEmpty(attr.Title) ? 0 : LabelHeight) + SpaceAfter;
    }

    public override void OnGUI(Rect position)
    {
        ShowDividerAttribute attr = (ShowDividerAttribute)attribute;

        float y = position.y + SpaceBefore;

        if (!string.IsNullOrEmpty(attr.Title))
        {
            var labelRect = new Rect(position.x, y, position.width, LabelHeight);
            EditorGUI.LabelField(labelRect, attr.Title, EditorStyles.boldLabel);
            y += LabelHeight;
        }

        var lineRect = new Rect(position.x, y, position.width, LineHeight);
        EditorGUI.DrawRect(lineRect, EditorColorPalette.Resolve(attr.Color));
    }
}