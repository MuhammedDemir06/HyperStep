using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoadManager : MonoBehaviour
{
    [Header("Target Tilemap")]
    public Tilemap targetTilemap;

    //For Now
    [Header("Referances")]
    [SerializeField] private LevelData levelData;

    public string tileResourcesPath = "Tiles/";
    private void Start()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        if (levelData == null || targetTilemap == null)
        {
            Debug.LogError($"❌ Level Data or Tilemap not Found.");
            return;
        }

        targetTilemap.ClearAllTiles();

        foreach (var tileData in levelData.Tiles)
        {
            TileBase tileAsset = Resources.Load<TileBase>(tileResourcesPath + "/" + tileData.TileName);

            if (tileAsset != null)
            {
                targetTilemap.SetTile(tileData.Position, tileAsset);
            }
            else
            {
                Debug.LogWarning($"TileAsset Not Found: '{tileData.TileName}'. Resources Folder '{tileResourcesPath}{tileData.TileName}' path need Check. this position skipped: {tileData.Position}");
            }
        }

        Debug.Log($"✅ Level Loaded: {levelData.name}");
    }
}
