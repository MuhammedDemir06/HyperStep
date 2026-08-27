using UnityEngine;
using UnityEngine.Audio;

public interface IAudioStateService
{
    bool IsSoundEnabled { get; }
    void ChangeAudioState();

    void UpdateMixer();
}

public class AudioStateService : IAudioStateService
{
    private AudioMixer audioMixer;

    private IPlayerData _playerData;

    public bool IsSoundEnabled => _playerData.CurrentPlayerData.IsSoundEnabled;

    public void Construct(AudioMixer newMixer,IPlayerData playerData)
    {
        audioMixer = newMixer;
        _playerData = playerData;
    }
    public void ChangeAudioState()
    {
        _playerData.CurrentPlayerData.IsSoundEnabled =! _playerData.CurrentPlayerData.IsSoundEnabled;

        UpdateMixer();
    }
    public void UpdateMixer()
    {
        bool isEnabled = _playerData.CurrentPlayerData.IsSoundEnabled;

        audioMixer.SetFloat("Master", isEnabled ? 0f : -80f);

        Debug.Log("Audio Enabled " + isEnabled);
    }
}
