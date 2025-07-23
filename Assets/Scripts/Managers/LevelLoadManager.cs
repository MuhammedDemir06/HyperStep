using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoadManager : MonoBehaviour
{
    [Header("Target Tilemap")]
    public Tilemap targetTilemap;

    //For Now
    [Header("Referances")]
    [SerializeField] private LevelData levelData;

    private void Start()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        if (levelData == null)
        {
            Debug.LogError($"❌ Level not found");
            return;
        }

        targetTilemap.ClearAllTiles();

        foreach (var tileData in levelData.tiles)
        {
            TileBase tile = tileData.TileAsset;
            targetTilemap.SetTile(tileData.Position, tile);
        }

        Debug.Log($"✅ Level loaded");
    }
}
