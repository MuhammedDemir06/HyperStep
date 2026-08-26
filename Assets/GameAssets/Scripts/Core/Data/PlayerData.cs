using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public bool IsSoundEnabled = true;
    public int CurrentChapter = 0;
    public int CurrentLevel = 0;
}
public interface IPlayerData
{
    PlayerData CurrentPlayerData { get; }
    void SaveData();
    void LoadData();
}

public class PlayerDataService : IPlayerData
{
    private PlayerData currentPlayerData = new PlayerData();
    public PlayerData CurrentPlayerData => currentPlayerData;

    private string filePath => Path.Combine(Application.persistentDataPath, "playerData.json");
    public void LoadData()
    {
        if (!File.Exists(filePath))
        {
            currentPlayerData = new PlayerData();
            SaveData();
            return;
        }

        string json = File.ReadAllText(filePath);

        currentPlayerData = JsonUtility.FromJson<PlayerData>(json);
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(currentPlayerData, true);

        File.WriteAllText(filePath, json);
    }
}
