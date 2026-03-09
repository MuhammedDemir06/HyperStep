using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private List<IPausable> pausables = new List<IPausable>();
    public bool GamePaused = false;
    public void PauseGame()
    {
        if (GamePaused)
            return;

        pausables.Clear();

        var allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var mono in allMonoBehaviours)
        {
            if (mono is IPausable pausable && pausable != null)
            {
                pausables.Add(pausable);
                pausable.OnPause();
            }
        }
        GamePaused = true;
    }
    public void ResumeGame()
    {
        if (!GamePaused)
            return;

        foreach (var p in pausables)
            p.OnResume();

        GamePaused = false;
    }
}