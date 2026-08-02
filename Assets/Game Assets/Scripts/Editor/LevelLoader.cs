using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class LevelLoader
{
    public static void LoadLevel(Sprite backgroundSprite,float timeLimit,LevelData levelData, Tilemap tilemap, Transform parent = null)
    {
        if (levelData == null || tilemap == null)
        {
            Debug.LogWarning("LevelData or Tilemap is null.");
            return;
        }

        RemoveLevel();
        tilemap.ClearAllTiles();

        foreach (var tileData in levelData.Tiles)
        {
            Tile tileAsset = Resources.Load<Tile>("Tiles/TileAsset Village/" + tileData.TileName);
            if (tileAsset != null)
            {
                tilemap.SetTile(tileData.Position, tileAsset);
            }
            else
            {
                Debug.LogWarning($"Tile '{tileData.TileName}' not found in Resources/Tiles.");
            }
        }

        foreach (var categoryData in levelData.Objects)
        {
            foreach (var objData in categoryData.Data)
            {
                GameObject prefab = Resources.Load<GameObject>(GetPrefabPathByCategory(categoryData.Category, objData.PrefabID));
                if (prefab != null)
                {
                    GameObject go = Object.Instantiate(prefab, objData.Position, objData.Rotation, parent);
                    go.name = objData.PrefabID;
                }
                else
                {
                    Debug.LogWarning($"Prefab '{objData.PrefabID}' not found for category '{categoryData.Category}'.");
                }
            }
        }

        timeLimit = levelData.LevelTimeLimit;
        backgroundSprite = levelData.BackgroundSprite;
    }
    private static string GetPrefabPathByCategory(LevelObjectCategory category, string prefabID)
    {
        switch (category)
        {
            case LevelObjectCategory.Normal: return $"Game/Normal/{prefabID}";
            case LevelObjectCategory.Trap: return $"Game/Traps/{prefabID}";
            case LevelObjectCategory.Enemy: return $"Game/Enemies/{prefabID}";
            case LevelObjectCategory.Finish:return $"Game/Finish/{prefabID}";
            case LevelObjectCategory.Spawn:return $"Game/Spawn/{prefabID}";
            default: return prefabID;
        }
    }
    public static void RemoveLevel(Transform parent = null)
    {
        GameObject[] allObjects;

        if (parent != null)
        {
            allObjects = parent.GetComponentsInChildren<Transform>(true)
                               .Select(t => t.gameObject)
                               .ToArray();
        }
        else
        {
            allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                               .Where(go => go.scene.IsValid())
                               .ToArray();
        }

        foreach (var obj in allObjects)
        {
            if (obj == null)
                continue;

            if (obj.CompareTag("Normal") || obj.CompareTag("Trap") || obj.CompareTag("Enemy") || obj.CompareTag("Finish") || obj.CompareTag("Spawn"))
            {
                Object.DestroyImmediate(obj);
            }
        }
    }


}
