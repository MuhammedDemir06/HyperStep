using UnityEngine;
using System.Collections.Generic;

public enum LevelObjectCategory
{
    Normal,
    Trap,
    Enemy
}

[System.Serializable]
public class LevelTileData
{
    public Vector3Int Position;
    public string TileName;
}
[System.Serializable]
public class LevelObjectData
{
    public string PrefabID;

    public Vector3 Position;
    public Quaternion Rotation;
}

[System.Serializable]
public class LevelCategoryData
{
    public LevelObjectCategory Category;
    public List<LevelObjectData> Data;
}
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Editor/Level Data")]
public class LevelData : ScriptableObject
{
    public string CurrentChapterName;
    public List<LevelTileData> Tiles = new List<LevelTileData>();
    public List<LevelCategoryData> Objects = new List<LevelCategoryData>();
}