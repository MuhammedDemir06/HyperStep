using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private List<IPausable> pausables = new List<IPausable>();
    public void PauseGame()
    {
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
    }
    public void ResumeGame()
    {
        foreach (var p in pausables)
            p.OnResume();
    }
}
