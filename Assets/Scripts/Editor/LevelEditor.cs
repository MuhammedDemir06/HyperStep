using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : EditorWindow
{
    private Tilemap targetTilemap;

    private bool isEditingLoadedLevel = false;
    private LevelData loadedLevelData = null;

    [MenuItem("Tools/Tilemap Level Saver")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Tilemap Saver");
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

        Load();

        Clear();

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

        bool canSave = targetTilemap != null && (!isEditingLoadedLevel || loadedLevelData != null);
        GUI.enabled = canSave;

        string buttonText = isEditingLoadedLevel ? "💾 Save Changes to Loaded Level" : "💾 Save New Level";

        if (GUILayout.Button(buttonText, GUILayout.Height(40)))
        {
            if (targetTilemap.cellSize.x < 1 || targetTilemap.cellSize.y < 1)
            {
                Debug.LogError("Invalid Tile size! Make sure the width and height of each cell are greater than 0.");
                return;
            }

            LevelSaver.SaveLevel(targetTilemap, loadedLevelData);
        }

        GUI.enabled = true;
        GUILayout.Space(10);
    }
    private void Load()
    {
        GUILayout.Space(10);

        loadedLevelData = (LevelData)EditorGUILayout.ObjectField("📂 Level to Load", loadedLevelData, typeof(LevelData), false);

        GUI.enabled = loadedLevelData != null && targetTilemap != null;

        if (GUILayout.Button("📥 Load Level", GUILayout.Height(40)))
        {
            LevelLoader.LoadLevel(loadedLevelData, targetTilemap);
            isEditingLoadedLevel = true;
            Debug.Log($"Level '{loadedLevelData.name}' loaded into scene for editing.");
        }

        GUI.enabled = true;
        GUILayout.Space(10);
    }

    private void Clear()
    {
        if (GUILayout.Button("🧹 Clear", GUILayout.Height(30)))
        {
            if (targetTilemap != null)
            {
                bool confirm = EditorUtility.DisplayDialog(
                    "Confirm Clear",
                    "Are you sure you want to clear?",
                    "Yes", "No");

                if (confirm)
                {
                    Undo.RecordObject(targetTilemap, "Clear");

                    targetTilemap.ClearAllTiles();

                    isEditingLoadedLevel = false;

                    loadedLevelData = null;

                    LevelLoader.RemoveLevel();

                    Debug.Log("Cleared.");
                }
            }
            else
            {
                Debug.LogWarning("No Tilemap assigned to clear.");
            }
        }
    }
}
