using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public static class TilemapSaver
{
    public static void SaveTilemap(Tilemap tilemap)
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap reference is null.");
            return;
        }

        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tileBase = tiles[x + y * bounds.size.x];

                if (tileBase != null && tileBase is Tile tile)
                {
                    Vector3Int position = new Vector3Int(x + bounds.x, y + bounds.y, 0);

                    level.tiles.Add(new TileData
                    {
                        Position = position,
                        TileAsset = tile
                    });
                }
            }
        }
        string path = "Assets/Resources/Levels/";
        string fileName = "Level";
        string assetPath = path + fileName + ".asset";

        AssetDatabase.CreateAsset(level, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ Level saved successfully: {assetPath}");
    }

}
