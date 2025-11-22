using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

public static class LevelSaver
{
    public static void SaveLevel(Tilemap tilemap,LevelData levelData)
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Null Tilemap Reference.");
            return;
        }

        LevelData newLevelData = new();

        if (levelData == null)
            newLevelData = ScriptableObject.CreateInstance<LevelData>();
        else
            newLevelData = levelData;


         SaveTilemap(tilemap, newLevelData);

        SaveObjects(newLevelData);

        SaveAsset(newLevelData, "Level");
    }
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
        Dictionary<LevelObjectCategory, List<LevelObjectData>> categoryDict = new Dictionary<LevelObjectCategory, List<LevelObjectData>>()
        {
            { LevelObjectCategory.Normal, new List<LevelObjectData>() },
            { LevelObjectCategory.Trap,   new List<LevelObjectData>() },
            { LevelObjectCategory.Enemy,  new List<LevelObjectData>() }
        };

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var obj in allObjects)
        {
            LevelObjectCategory? category = null;

            if (obj.CompareTag("Normal")) category = LevelObjectCategory.Normal;
            else if (obj.CompareTag("Trap")) category = LevelObjectCategory.Trap;
            else if (obj.CompareTag("Enemy")) category = LevelObjectCategory.Enemy;

            if (category.HasValue)
            {
                string prefabID = obj.name.Replace("(Clone)", "").Trim();

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
            LevelCategoryData catData = new LevelCategoryData
            {
                Category = kvp.Key,
                Data = kvp.Value
            };
            levelData.Objects.Add(catData);
        }
    }
    private static void SaveAsset(LevelData levelData, string levelName)
    {
        string path = "Assets/Resources/Levels/";
        string assetPath = path + levelName + ".asset";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.CreateAsset(levelData, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ New Level Saved with tiles and objects: {assetPath}");
    }
}
