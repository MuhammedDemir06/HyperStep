using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public enum SceneType
{
    Menu = 0,
    Game = 1
}
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("UI References")]
    [SerializeField] private Image transitionImage;    
    [SerializeField] private GameObject loadingGroup;
    [SerializeField] private GameObject transitionUI;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        FadeIn(); 
    }
    public void LoadScene(SceneType sceneType)
    {
        StartCoroutine(LoadSceneAsync(sceneType));
    }
    private void DeactivateTransitionUI(bool state)
    {
        transitionUI.SetActive(state);
    }
    private IEnumerator LoadSceneAsync(SceneType sceneType)
    {
        DeactivateTransitionUI(true);

        yield return transitionImage.DOFade(1f, fadeDuration).WaitForCompletion();

        loadingGroup.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)sceneType);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;

        yield return new WaitForSeconds(0.5f);

        loadingGroup.SetActive(false);
        yield return transitionImage.DOFade(0f, fadeDuration).WaitForCompletion();

        DeactivateTransitionUI(false);

        
    }
    private void FadeIn()
    {
        transitionImage.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            loadingGroup.SetActive(false);
            DeactivateTransitionUI(false);
        });
    }
    public void QuitGame()
    {
        StartCoroutine(QuitAsync());
    }
    private IEnumerator QuitAsync()
    {
        DeactivateTransitionUI(true);

        yield return transitionImage.DOFade(1f, fadeDuration).WaitForCompletion();
        loadingGroup.SetActive(true);

        yield return new WaitForSeconds(fadeDuration);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
