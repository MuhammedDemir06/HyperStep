using UnityEngine;
using UnityEditor;
using System.Reflection;
using IronTools.Attributes;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(MonoBehaviour), true)]
public class SpecialButtonEditor : Editor
{
    private Dictionary<string, object[]> methodParams = new();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<ShowButtonAttribute>();
            if (attr == null) continue;

            string label = string.IsNullOrEmpty(attr.ButtonLabel) ? method.Name : attr.ButtonLabel;
            Texture2D icon = null;
            if (!string.IsNullOrEmpty(attr.IconPath))
                icon = Resources.Load<Texture2D>(attr.IconPath);

            GUIContent content = new GUIContent(attr.OnlyIcon ? "" : label, icon, label);
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                imagePosition = attr.OnlyIcon ? ImagePosition.ImageOnly : ImagePosition.ImageLeft,
                fixedHeight = 32
            };

            // Parametreleri al
            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                if (!methodParams.ContainsKey(method.Name))
                    methodParams[method.Name] = new object[parameters.Length];

                EditorGUILayout.BeginVertical("box");

                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    Type type = param.ParameterType;

                    object currentValue = methodParams[method.Name][i];

                    if (type == typeof(int))
                        methodParams[method.Name][i] = EditorGUILayout.IntField(param.Name, currentValue != null ? (int)currentValue : 0);
                    else if (type == typeof(float))
                        methodParams[method.Name][i] = EditorGUILayout.FloatField(param.Name, currentValue != null ? (float)currentValue : 0f);
                    else if (type == typeof(string))
                        methodParams[method.Name][i] = EditorGUILayout.TextField(param.Name, currentValue?.ToString() ?? "");
                    else if (type == typeof(Vector3))
                        methodParams[method.Name][i] = EditorGUILayout.Vector3Field(param.Name, currentValue != null ? (Vector3)currentValue : Vector3.zero);
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                        methodParams[method.Name][i] = EditorGUILayout.ObjectField(param.Name, currentValue as UnityEngine.Object, type, true);
                    else
                        EditorGUILayout.LabelField(param.Name + " (Unsupported Type)");
                }

                if (GUILayout.Button(content, buttonStyle))
                {
                    method.Invoke(target, methodParams[method.Name]);
                }

                EditorGUILayout.EndVertical();
            }
            else
            {
                if (GUILayout.Button(content, buttonStyle))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }
}

