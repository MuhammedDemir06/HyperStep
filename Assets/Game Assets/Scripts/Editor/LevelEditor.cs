using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : EditorWindow
{
    private Tilemap targetTilemap;

    private bool isEditingLoadedLevel = false;
    private LevelData loadedLevelData = null;

    private Vector2 prefabScroll;
    private const int iconSize = 64;
    private const int padding = 10;

    private Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();

    private Vector2 mainScroll;

    private bool showLevelInfo = true;

    private string chapterName = "NewChapter";

    [MenuItem("Tools/ Level Saver")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Saver");
    }
    private void Title()
    {
        GUILayout.Space(10);
        GUILayout.Label("🧱 Level Saver", TextStyle(20, FontStyle.Bold, TextAnchor.MiddleCenter));
        GUILayout.Space(5);
    }
    private void OnGUI()
    {
        mainScroll = EditorGUILayout.BeginScrollView(mainScroll);

        Title();

        TilemapReferance();

        ChapterCounter();

        Save();
        Load();
        Clear();

        EditorGUILayout.LabelField("📂 Save Path:", "Assets/Resources", EditorStyles.miniBoldLabel);

        if (targetTilemap == null)
        {
            EditorGUILayout.HelpBox("Please assign a Tilemap before saving.", MessageType.Warning);
        }

        DrawPrefabGallery();

        EditorGUILayout.EndScrollView();
    }
    private void ChapterCounter()
    {
        GUILayout.Space(10);

        float buttonWidth = 200;
        Rect buttonRect = GUILayoutUtility.GetRect(buttonWidth, 30);

        if (GUI.Button(buttonRect, showLevelInfo ? "▼ Chapters Info" : "▶ Chapters Info", TextStyle(15, FontStyle.Normal, TextAnchor.MiddleCenter)))
        {
            showLevelInfo = !showLevelInfo;
        }

        if (showLevelInfo)
        {
            EditorGUI.indentLevel++;

            string chaptersPath = "Assets/Resources/Chapter Data/Chapters";
            GUIStyle centeredStyle = TextStyle(12, FontStyle.Normal, TextAnchor.MiddleCenter);

            if (System.IO.Directory.Exists(chaptersPath))
            {
                string[] chapterDirs = System.IO.Directory.GetDirectories(chaptersPath);

                GUILayout.Label($"Total Chapters Created: {chapterDirs.Length}", centeredStyle);
                GUILayout.Space(5);

                foreach (var chapterDir in chapterDirs)
                {
                    string dirName = System.IO.Path.GetFileName(chapterDir);

                    if (!string.IsNullOrEmpty(chapterName) && dirName == chapterName)
                    {
                        string[] levelFiles = System.IO.Directory.GetFiles(chapterDir, "*.asset", System.IO.SearchOption.TopDirectoryOnly);
                        int levelCount = levelFiles.Length;

                        GUILayout.Label($"Chapter: {dirName} | Levels: {levelCount}", centeredStyle);
                    }
                }
            }
            else
            {
                GUILayout.Label("No Chapters Found", centeredStyle);
            }

            EditorGUI.indentLevel--;
            GUILayout.Space(10);
        }

    }
    private void DrawPrefabGallery()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("🎛 Prefab Browser", TextStyle(15, FontStyle.Normal, TextAnchor.MiddleCenter));

        prefabScroll = EditorGUILayout.BeginScrollView(prefabScroll, GUILayout.Height(300));

        var prefabs = PrefabGalleryUtility.GetPrefabs();

        foreach (var category in prefabs)
        {
            string catName = category.Key;

            if (!foldoutStates.ContainsKey(catName))
                foldoutStates[catName] = false;

            EditorGUILayout.Space(4);

            foldoutStates[catName] = EditorGUILayout.Foldout(
                foldoutStates[catName],
                "▶ " + catName,
                true,
                EditorStyles.foldoutHeader
            );

            if (!foldoutStates[catName])
                continue;

            int rowCount = Mathf.FloorToInt((EditorGUIUtility.currentViewWidth - 40) / (iconSize + padding));
            if (rowCount < 1) rowCount = 1;

            int index = 0;
            EditorGUILayout.BeginHorizontal();

            foreach (var prefab in category.Value)
            {
                if (index >= rowCount)
                {
                    index = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }

                DrawPrefabButton(prefab);
                index++;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.EndScrollView();
    }
    private void DrawPrefabButton(GameObject prefab)
    {
        if (prefab == null)
        {
            GUILayout.Label("NULL Prefab");
            return;
        }

        Texture2D preview = AssetPreview.GetAssetPreview(prefab);
        if (preview == null)
            preview = AssetPreview.GetMiniThumbnail(prefab);

        GUILayout.BeginVertical(GUILayout.Width(iconSize));

        Rect rect = GUILayoutUtility.GetRect(iconSize, iconSize);

        GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit);

        EditorGUILayout.LabelField(prefab.name, EditorStyles.miniLabel, GUILayout.Width(iconSize));

        Event evt = Event.current;
        if (rect.Contains(evt.mousePosition))
        {
            if (evt.type == EventType.MouseDown && evt.button == 0)
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new Object[] { prefab };
                DragAndDrop.StartDrag(prefab.name);
                evt.Use();
            }
        }

        GUILayout.EndVertical();
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

        GUILayout.Label("🧾 Save Level", TextStyle(15, FontStyle.Normal, TextAnchor.MiddleCenter));

        GUILayout.Space(10);

        chapterName = EditorGUILayout.TextField("Chapter Name", chapterName);

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

            LevelSaver.SaveLevel(targetTilemap, loadedLevelData,chapterName);
        }
        if (GUILayout.Button("Remove Chapter", GUILayout.Height(40)))
        {
            if (string.IsNullOrEmpty(chapterName))
            {
                Debug.LogWarning("Please enter a Chapter Name to remove.");
            }
            else
            {
                bool confirm = EditorUtility.DisplayDialog(
                    "Confirm Delete",
                    $"Are you sure you want to delete the Chapter '{chapterName}'?\nThis will also delete all levels inside it!",
                    "Yes",
                    "No"
                );

                if (confirm)
                {
                    LevelSaver.DeleteChapter(chapterName);
                }
            }
        }

        GUI.enabled = true;
        GUILayout.Space(10);
    } 
    public GUIStyle TextStyle(int newFontSize, FontStyle style, TextAnchor anchor)
    {
        return new GUIStyle
        {
            fontSize = newFontSize,
            fontStyle = style,
            alignment = anchor,

            normal = new GUIStyleState
            {
                textColor = Color.white
            }
        };
    }
    private void Load()
    {
        GUILayout.Space(10);

        GUILayout.Label("🔁 Load Level", TextStyle(15, FontStyle.Normal, TextAnchor.MiddleCenter));
        GUILayout.Space(10);

        loadedLevelData = (LevelData)EditorGUILayout.ObjectField("📂 Level to Load", loadedLevelData, typeof(LevelData), false);

        bool canLoad = loadedLevelData != null && targetTilemap != null;

        GUI.enabled = canLoad;
        if (GUILayout.Button("📥 Load Level", GUILayout.Height(40)))
        {
            LevelLoader.LoadLevel(loadedLevelData, targetTilemap);
            isEditingLoadedLevel = true;
            Debug.Log($"Level '{loadedLevelData.name}' loaded into scene for editing.");
        }
        GUI.enabled = true;

        GUILayout.Space(5);

        GUI.enabled = loadedLevelData != null;
        if (GUILayout.Button("🗑 Delete Level", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Confirm Delete",
                $"Are you sure you want to delete level '{loadedLevelData.name}'?",
                "Yes", "No"))
            {
                LevelSaver.DeleteLevel(loadedLevelData,chapterName);
                loadedLevelData = null;
                isEditingLoadedLevel = false;
            }
        }
        GUI.enabled = true;

        GUILayout.Space(10);
    }
    private void Clear()
    {
        GUILayout.Space(10);

        GUILayout.Label("🧹 Clear Level", TextStyle(15, FontStyle.Normal, TextAnchor.MiddleCenter));

        GUILayout.Space(10);

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
