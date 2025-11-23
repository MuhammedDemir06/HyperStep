using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Chapter
{
    public string ChapterName;
    public List<LevelData> Levels = new List<LevelData>();
}

[CreateAssetMenu(fileName = "NewChapter", menuName = "Level Editor/Chapter")]
public class ChapterData : ScriptableObject
{
    public List<Chapter> Chapters = new List<Chapter>();
}