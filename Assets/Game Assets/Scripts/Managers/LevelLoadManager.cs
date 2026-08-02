using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class LevelLoadManager : MonoBehaviour
{
    [Header("Target Tilemap")]
    public Tilemap targetTilemap;
    [SerializeField] private Transform spawnParent;

    [Header("Level Info")]
    [SerializeField] private ChapterData chapterData;
    [SerializeField] private int chapterIndex;
    [SerializeField] private int levelIndex;

    [SerializeField] private SpriteRenderer background;

    [Header("Tile Resources Path")]
    public string tileResourcesPath = "Tiles/";

    public int LevelTime = 0;

    private IGameStateService _gameStateService;
    private PlayerController _playerController;

    [Header("Death Offset")]
    [SerializeField] private float maxDeathOffset = 4f; 
    private float deathY;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-1000, deathY, 0), new Vector3(1000, deathY, 0));
    }
    public void Construct(IGameStateService gameStateService,PlayerController playerController)
    {
        _gameStateService = gameStateService;
        _playerController = playerController;

        GameStart();
        CalculateDeathHeight();

        background.transform.SetParent(_playerController.transform);
        _playerController.SetPlayerStartPos(PlayerStartPos());
        playerController.SetDeathOffset(deathY);
    }
    private void GameStart()
    {
        _gameStateService.ChangeState(GameState.GamePlay);

        LoadLevel();
    }
    public void LoadLevel()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("❌ Target Tilemap not assigned.");
            return;
        }

        LevelData levelData = chapterData.Chapters[chapterIndex].Levels[levelIndex];

        if (chapterData == null || levelData == null)
        {
            Debug.LogError($"❌ Chapter or Level not found.");
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
                {
                    var spawned = Instantiate(prefab, objData.Position, objData.Rotation, spawnParent);

                    if (spawned.TryGetComponent(out ILevelInitializable init))
                    {
                        init.Initialize(_gameStateService);
                    }
                }
                else
                    Debug.LogWarning($"⚠️ Prefab '{objData.PrefabID}' not found at '{prefabPath}'");
            }
        }
        LevelTime = levelData.LevelTimeLimit;
        background.sprite = levelData.BackgroundSprite;
    }
    private void CalculateDeathHeight()
    {
        targetTilemap.CompressBounds();
        BoundsInt bounds = targetTilemap.cellBounds;

        int minY = int.MaxValue;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (targetTilemap.HasTile(pos))
            {
                if (pos.y < minY) minY = pos.y;
            }
        }

        if (minY == int.MaxValue)
        {
            Debug.LogWarning("No Tile");
            deathY = -50f;
            return;
        }

        deathY = targetTilemap.CellToWorld(new Vector3Int(0, minY, 0)).y - maxDeathOffset;
    }
    private Vector3 PlayerStartPos()
    {
        LevelData levelData = chapterData.Chapters[chapterIndex].Levels[levelIndex];

        if (levelData == null)
        {
            Debug.LogError($"❌ Chapter or Level not found.");

            return Vector3.zero;
        }

        foreach (var category in levelData.Objects)
        {
            if (category.Category != LevelObjectCategory.Spawn)
                continue;

            if (category.Data.Count == 0)
                break;

            return category.Data[0].Position;
        }

        Debug.LogError("Spawn object not found in LevelData.");
        return Vector3.zero;
    }
    private string GetPrefabPathByCategory(LevelObjectCategory category, string prefabID)
    {
        return category switch
        {
            LevelObjectCategory.Normal => $"Game/Normal/{prefabID}",
            LevelObjectCategory.Trap => $"Game/Traps/{prefabID}",
            LevelObjectCategory.Enemy => $"Game/Enemies/{prefabID}",
            LevelObjectCategory.Finish => $"Game/Finish/{prefabID}",
            LevelObjectCategory.Spawn => $"Game/Spawn/{prefabID}",
            _ => prefabID
        };
    }
}
