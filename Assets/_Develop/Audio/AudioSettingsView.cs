using TMPro;
using UnityEngine;

public class AudioSettingsView : MonoBehaviour
{
    private AudioSettings _audioSettings;

    [SerializeField] private TMP_Text _infoText;

    public void Initialize(AudioSettings audioSettings)
    { 
        _audioSettings = audioSettings;
    }

    private void Update()
    {
        ShowSoundLevels();
    }
    private void ShowSoundLevels()
    {
        _infoText.text = "Music : " + (_audioSettings.MusicEnabled ? "ON" : "OFF") +
            "\nSFX : " + (_audioSettings.SFXEnabled ? "ON" : "OFF");
    }

    public void ToggleSFX()
    {
        _audioSettings.ToggleSFX();
    }

    public void ToggleMusic()
    {
        _audioSettings.ToggleMusic();
    }
}
