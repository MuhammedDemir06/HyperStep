using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : EditorWindow
{
    private Tilemap targetTilemap;

    [MenuItem("Tools/Tilemap Level Saver")]
    public static void ShowWindow()
    {
        GetWindow<TilemapEditor>("Tilemap Saver");
    }
    private void Title()
    {
        GUILayout.Space(10);
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("🧱 Tilemap Level Saver", headerStyle);
        GUILayout.Space(5);
    }
    private void OnGUI()
    {
        Title();

        EditorGUILayout.HelpBox("Save your tilemap as a ScriptableObject level into Resources/Levels.", MessageType.Info);

        TilemapReferance();

        Save();

        ClearTilemap();

        EditorGUILayout.LabelField("📂 Save Path:", "Assets/Resources/Levels", EditorStyles.miniBoldLabel);

        if (targetTilemap == null)
        {
            EditorGUILayout.HelpBox("Please assign a Tilemap before saving.", MessageType.Warning);
        }
    }
    private void TilemapReferance()
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginVertical("box");
        targetTilemap = (Tilemap)EditorGUILayout.ObjectField("🎯 Target Tilemap", targetTilemap, typeof(Tilemap), true);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
    }
    private void Save()
    {
        GUILayout.Space(10);

        GUI.enabled = targetTilemap != null;
        if (GUILayout.Button("💾 Save Tilemap as Level", GUILayout.Height(40)))
        {
            if(targetTilemap.cellSize.x<1 || targetTilemap.cellSize.y<1)
            {
                Debug.LogError("Invalid tile size! Make sure each cell's width and height are greater than 0.");
                return;
            }

            TilemapSaver.SaveTilemap(targetTilemap);
        }
        GUI.enabled = true;

        GUILayout.Space(10);
    }
    private void ClearTilemap()
    {
        if (GUILayout.Button("🧹 Clear Tilemap", GUILayout.Height(30)))
        {
            if (targetTilemap != null)
            {
                bool confirm = EditorUtility.DisplayDialog(
                    "Confirm Clear",
                    "Are you sure you want to clear the tilemap?",
                    "Yes", "No");

                if (confirm)
                {
                    Undo.RecordObject(targetTilemap, "Clear Tilemap");
                    targetTilemap.ClearAllTiles();
                    Debug.Log("Tilemap cleared.");
                }
            }
            else
            {
                Debug.LogWarning("No Tilemap assigned to clear.");
            }
        }
    }
}
