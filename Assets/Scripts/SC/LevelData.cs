using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileData
{
    public Vector3Int Position;
    public Tile TileAsset;
}
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Editor/Level Data")]
public class LevelData : ScriptableObject
{
    public List<TileData> tiles = new List<TileData>();
}