using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class LevelSaver
{
    private static void SaveTilemap(Tilemap tilemap, LevelData levelData)
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        for (int y = 0; y < bounds.size.y; y++)
        {
            for (int x = 0; x < bounds.size.x; x++)
            {
                TileBase tileBase = tiles[x + y * bounds.size.x];

                if (tileBase != null)
                {
                    string tileName = "";

                    if (tileBase is Tile tile)
                    {
                        string asset = AssetDatabase.GetAssetPath(tile);
                        tileName = Path.GetFileNameWithoutExtension(asset);
                    }
                    else
                    {
                        tileName = tileBase.name;
                    }

                    Vector3Int position = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                    levelData.Tiles.Add(new LevelTileData
                    {
                        Position = position,
                        TileName = tileName
                    });
                }
            }
        }
    }
    private static void SaveObjects(LevelData levelData)
    {
        levelData.Objects.Clear();

        Dictionary<LevelObjectCategory, List<LevelObjectData>> categoryDict = new Dictionary<LevelObjectCategory, List<LevelObjectData>>()
    {
        { LevelObjectCategory.Normal, new List<LevelObjectData>() },
        { LevelObjectCategory.Trap,   new List<LevelObjectData>() },
        { LevelObjectCategory.Enemy,  new List<LevelObjectData>() }
    };

        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                                        .Where(go => go.scene.IsValid())
                                        .ToArray();

        foreach (var obj in allObjects)
        {
            LevelObjectCategory? category = null;

            if (obj.CompareTag("Normal")) category = LevelObjectCategory.Normal;
            else if (obj.CompareTag("Trap")) category = LevelObjectCategory.Trap;
            else if (obj.CompareTag("Enemy")) category = LevelObjectCategory.Enemy;

            if (category.HasValue)
            {
                string prefabID = obj.name.Replace("(Clone)", "").Trim();
                prefabID = Regex.Replace(prefabID, @"\s*\(\d+\)$", "");

                categoryDict[category.Value].Add(new LevelObjectData
                {
                    PrefabID = prefabID,
                    Position = obj.transform.position,
                    Rotation = obj.transform.rotation
                });
            }
        }

        foreach (var kvp in categoryDict)
        {
            if (kvp.Value.Count > 0)
            {
                LevelCategoryData catData = new LevelCategoryData
                {
                    Category = kvp.Key,
                    Data = kvp.Value
                };
                levelData.Objects.Add(catData);
            }
        }
    }
    public static void SaveLevel(Tilemap tilemap, LevelData levelData, string chapterName,int timeLimit)
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Null Tilemap Reference.");
            return;
        }

        LevelData newLevelData = levelData != null ? levelData : ScriptableObject.CreateInstance<LevelData>();

        newLevelData.CurrentChapterName = chapterName;
        newLevelData.LevelTimeLimit = timeLimit;

        SaveTilemap(tilemap, newLevelData);
        SaveObjects(newLevelData);

        SaveAsset(newLevelData, chapterName);
    }
    private static void SaveAsset(LevelData levelData, string chapterName)
    {
        string basePath = "Assets/Resources/Chapter Data/Chapters";

        if (string.IsNullOrEmpty(chapterName))
            chapterName = "NewChapter";

        chapterName = SanitizeFileName(chapterName);
        string chapterPath = Path.Combine(basePath, chapterName);

        if (!Directory.Exists(chapterPath))
            Directory.CreateDirectory(chapterPath);

        string[] existingLevels = Directory.GetFiles(chapterPath, $"{chapterName}_Level_*.asset", SearchOption.TopDirectoryOnly);
        int levelIndex = existingLevels.Length;

        string assetPath = Path.Combine(chapterPath, $"{chapterName}_Level_{levelIndex}.asset");

        AssetDatabase.CreateAsset(levelData, assetPath);
        AssetDatabase.SaveAssets();

        AddLevelToChapterData(levelData, chapterName);

        Debug.Log($"✅ Level '{levelData.name}' saved in Chapter '{chapterName}' at {assetPath}");
    }
    private static string SanitizeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
    private static void AddLevelToChapterData(LevelData levelData, string chapterName)
    {
        string chapterDataPath = "Assets/Resources/Chapter Data/ChaptersData.asset";
        ChapterData chapterData = AssetDatabase.LoadAssetAtPath<ChapterData>(chapterDataPath);

        if (chapterData == null)
        {
            chapterData = ScriptableObject.CreateInstance<ChapterData>();
            if (!Directory.Exists("Assets/Resources/Chapter Data"))
                Directory.CreateDirectory("Assets/Resources/Chapter Data");

            AssetDatabase.CreateAsset(chapterData, chapterDataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Chapter chapter = chapterData.Chapters.Find(c => c.ChapterName == chapterName);
        if (chapter == null)
        {
            chapter = new Chapter
            {
                ChapterName = chapterName,
                Levels = new List<LevelData>()
            };
            chapterData.Chapters.Add(chapter);
        }
//Add Level
        if (!chapter.Levels.Contains(levelData))
        {
            chapter.Levels.Add(levelData);
            EditorUtility.SetDirty(chapterData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"✅ Level '{levelData.name}' added to Chapter '{chapterName}' in ChapterData.");
        }
    }
    public static void DeleteChapter(string chapterName)
    {
        if (string.IsNullOrEmpty(chapterName))
        {
            Debug.LogError("Chapter name is empty!");
            return;
        }
        string chapterDataPath = "Assets/Resources/Chapter Data/ChaptersData.asset";
        ChapterData chapterData = AssetDatabase.LoadAssetAtPath<ChapterData>(chapterDataPath);
        if (chapterData != null)
        {
            Chapter targetChapter = chapterData.Chapters.Find(c => c.ChapterName == chapterName);
            if (targetChapter != null)
            {
                chapterData.Chapters.Remove(targetChapter);
                EditorUtility.SetDirty(chapterData);
                AssetDatabase.SaveAssets();
                Debug.Log($"Removed Chapter '{chapterName}' from ChapterData.asset");
            }
            else
            {
                Debug.LogWarning($"Chapter '{chapterName}' not found in ChapterData.asset");
            }
        }
        string chapterFolderPath = $"Assets/Resources/Chapter Data/Chapters/{chapterName}";
        if (System.IO.Directory.Exists(chapterFolderPath))
        {
            bool result = AssetDatabase.DeleteAsset(chapterFolderPath);
            if (result)
                Debug.Log($"Deleted Chapter folder: {chapterFolderPath}");
            else
                Debug.LogError($"Failed to delete Chapter folder: {chapterFolderPath}");
        }
        else
        {
            Debug.LogWarning($"Chapter folder not found!");
        }
    }
    public static void DeleteLevel(LevelData level,string chapterName)
    {
        LevelLoader.RemoveLevel();

        chapterName = level.CurrentChapterName;

        if (string.IsNullOrEmpty(chapterName))
        {
            Debug.LogError("Chapter Not Found!");
            return;
        }

        string chapterDataPath = "Assets/Resources/Chapter Data/ChaptersData.asset";
        ChapterData chapterData = AssetDatabase.LoadAssetAtPath<ChapterData>(chapterDataPath);
        if (chapterData != null)
        {
            Chapter targetChapter = chapterData.Chapters.Find(c => c.ChapterName == chapterName);
            if (targetChapter != null)
            {
                LevelData levelToRemove = targetChapter.Levels.Find(l => l.name == level.name);
                if (levelToRemove != null)
                {
                    targetChapter.Levels.Remove(levelToRemove);
                    EditorUtility.SetDirty(chapterData);
                    AssetDatabase.SaveAssets();
                    Debug.Log($"Removed '{levelToRemove.name}' from ChapterData '{chapterName}'");
                }
            }
        }

        string chapterPath = $"Assets/Resources/Chapter Data/Chapters/{chapterName}";
        string[] guids = AssetDatabase.FindAssets(level.name + " t:LevelData", new[] { chapterPath });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.DeleteAsset(path);
            Debug.Log($"Deleted Level Asset: {path}");
        }
    }
}