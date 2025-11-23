using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LevelLoadManager : MonoBehaviour
{
    [Header("Target Tilemap")]
    public Tilemap targetTilemap;
    [SerializeField] private Transform spawnParent;

    [Header("Level Info")]
    public string chapterName;
    public int levelIndex = 0;

    [Header("Tile Resources Path")]
    public string tileResourcesPath = "Tiles/";

    private void Start()
    {
        LoadLevel(chapterName, levelIndex);
    }

    public void LoadLevel(string chapter, int index)
    {
        if (targetTilemap == null)
        {
            Debug.LogError("❌ Target Tilemap not assigned.");
            return;
        }

        if (string.IsNullOrEmpty(chapter))
        {
            Debug.LogError("❌ Chapter name is empty.");
            return;
        }

        // Chapter path
        string chapterPath = $"Chapter Data/Chapters/{chapter}/{chapter}_Level_{index}";
        LevelData levelData = Resources.Load<LevelData>(chapterPath);

        if (levelData == null)
        {
            Debug.LogError($"❌ Level not found at Resources/{chapterPath}. Check Chapter name and Level index.");
            return;
        }

        // Clear Tilemap
        targetTilemap.ClearAllTiles();

        // Spawn Tiles
        foreach (var tileData in levelData.Tiles)
        {
            TileBase tileAsset = Resources.Load<TileBase>(tileResourcesPath + tileData.TileName);
            if (tileAsset != null)
                targetTilemap.SetTile(tileData.Position, tileAsset);
            else
                Debug.LogWarning($"⚠️ Tile '{tileData.TileName}' not found. Skipped: {tileData.Position}");
        }

        // Clear previous spawned objects
        if (spawnParent != null)
        {
            for (int i = spawnParent.childCount - 1; i >= 0; i--)
                Destroy(spawnParent.GetChild(i).gameObject);
        }

        // Spawn objects
        foreach (var categoryData in levelData.Objects)
        {
            foreach (var objData in categoryData.Data)
            {
                string prefabPath = GetPrefabPathByCategory(categoryData.Category, objData.PrefabID);
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab != null)
                    Instantiate(prefab, objData.Position, objData.Rotation, spawnParent);
                else
                    Debug.LogWarning($"⚠️ Prefab '{objData.PrefabID}' not found at '{prefabPath}'");
            }
        }

        Debug.Log($"✅ Level Loaded: Chapter '{chapter}', Level {index}");
    }
    private string GetPrefabPathByCategory(LevelObjectCategory category, string prefabID)
    {
        return category switch
        {
            LevelObjectCategory.Normal => $"Game/Normal/{prefabID}",
            LevelObjectCategory.Trap => $"Game/Traps/{prefabID}",
            LevelObjectCategory.Enemy => $"Game/Enemies/{prefabID}",
            _ => prefabID
        };
    }
}
