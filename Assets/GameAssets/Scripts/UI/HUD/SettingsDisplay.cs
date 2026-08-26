using IronTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDisplay : MonoBehaviour
{
    [ShowDivider(EditorColor.Green, "Setting UI")]
    [Header("Back Button")]
    [SerializeField] private Button backButton;
    [Header("Sound Button")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Sprite soundButtonEnabledSprite;
    [SerializeField] private Sprite soundButtonDisabledSprite;

    private AnimatedPanel panel;

    private IAudioStateService _audioStateService;
    private void Awake()
    {
        panel = GetComponent<AnimatedPanel>();
    }
    private void Start()
    {
        backButton.onClick.AddListener(Back);
        soundButton.onClick.AddListener(ToggleSound);
    }
    public void Construct(IAudioStateService audioStateService)
    {
        _audioStateService = audioStateService;

        SetSoundButtonSprite(_audioStateService.IsSoundEnabled);
    }
    private void ToggleSound()
    {
        _audioStateService.ChangeAudioState();

        SetSoundButtonSprite(_audioStateService.IsSoundEnabled);
    }
    private void SetSoundButtonSprite(bool value)
    {
        if (value)
            soundButtonImage.sprite = soundButtonEnabledSprite;
        else
            soundButtonImage.sprite = soundButtonDisabledSprite;
    }
    private void Back()
    {
        Hide();
    }
    public void Show() => panel.Show();
    public void Hide() => panel.Hide();
}
