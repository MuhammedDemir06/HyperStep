using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[InitializeOnLoad]
public static class PrefabGalleryUtility
{
    private static readonly string basePath = "Assets/Resources/Game/";
    private static Dictionary<string, List<GameObject>> cachedPrefabs = new();

    static PrefabGalleryUtility()
    {
        EditorApplication.projectChanged += RefreshPrefabs;
    }

    public static void RefreshPrefabs()
    {
        cachedPrefabs.Clear();
        LoadCategory("Normal");
        LoadCategory("Traps");
        LoadCategory("Enemies");
    }

    private static void LoadCategory(string category)
    {
        string fullPath = basePath + category;

        if (!cachedPrefabs.ContainsKey(category))
            cachedPrefabs.Add(category, new List<GameObject>());

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { fullPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null)
                cachedPrefabs[category].Add(prefab);
        }
    }

    public static Dictionary<string, List<GameObject>> GetPrefabs()
    {
        if (cachedPrefabs.Count == 0)
            RefreshPrefabs();

        return cachedPrefabs;
    }
}
