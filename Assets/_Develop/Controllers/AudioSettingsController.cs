using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsController : Controller
{
    private AudioSettings _audioSettings;
    
    public AudioSettingsController(AudioSettings audioSettings)
    {
        _audioSettings = audioSettings;
    }

    public void InitializeController()
    {
        _audioSettings.Initialize();
    }

    protected override void UpdateLogic(float deltaTime)
    {
    }
}
