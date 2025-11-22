namespace IronTools.Attributes
{
    using System;
    using UnityEngine;
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionFieldName;
        public ShowIfAttribute(string conditionFieldName)
        {
            ConditionFieldName = conditionFieldName;
        }
    }
    public class ShowButtonAttribute : Attribute
    {
        public string ButtonLabel;
        public string IconPath;
        public bool OnlyIcon;

        public ShowButtonAttribute(string label = null, string iconPath = null, bool onlyIcon = false)
        {
            ButtonLabel = label;
            IconPath = iconPath;
            OnlyIcon = onlyIcon;
        }
    }
    public class ShowTitleAttribute : PropertyAttribute
    {
        public string title;
        public EditorColor color;

        public ShowTitleAttribute(string title)
        {
            this.title = title;
            this.color = EditorColor.White;
        }
        public ShowTitleAttribute(string title, EditorColor color)
        {
            this.title = title;
            this.color = color;
        }
    }
    public class SectionAttribute : PropertyAttribute
    {
        public EditorColor ColorEnum;
        public SectionAttribute(EditorColor color = EditorColor.White)
        {
            ColorEnum = color;
        }
    }
    public class ShowDividerAttribute : PropertyAttribute
    {
        public string Title;
        public EditorColor Color;

        public ShowDividerAttribute(EditorColor color = EditorColor.White, string title = "")
        {
            Color = color;
            Title = title;
        }
    }
    public class EditorColorPalette
    {
        public static Color Resolve(EditorColor color)
        {
            return color switch
            {
                EditorColor.Red => new Color(1f, 0.3f, 0.3f),
                EditorColor.Green => new Color(0.4f, 1f, 0.4f),
                EditorColor.Blue => new Color(0.4f, 0.7f, 1f),
                EditorColor.Yellow => new Color(1f, 1f, 0.3f),
                EditorColor.Orange => new Color(1f, 0.6f, 0.2f),
                EditorColor.Cyan => new Color(0.3f, 1f, 1f),
                EditorColor.Purple => new Color(0.8f, 0.6f, 1f),
                _ => Color.white
            };
        }
    }
}
public enum EditorColor
{
    White,
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Cyan,
    Purple
}
