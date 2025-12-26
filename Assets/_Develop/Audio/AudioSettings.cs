using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings
{
    private const string MusicParam = "MusicVolume";
    private const string SFXParam = "SFXVolume";
    private const float SilentMusicDB = -80f;
    private const float SilentSFXDB = -80f;

    private AudioMixer _audioMixer;
    private bool _musicEnabled = true;
    private bool _sfxEnabled = true;

    private float NormalMusicDB;
    private float NormalSFXDB;

    public bool MusicEnabled => _musicEnabled;
    public bool SFXEnabled => _sfxEnabled;

    public AudioSettings(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }

    public void Initialize()
    {
        _audioMixer.GetFloat(MusicParam, out NormalMusicDB);
        _audioMixer.GetFloat(SFXParam, out NormalSFXDB);
    }

    public void ToggleMusic()
    {
        _audioMixer.GetFloat(MusicParam, out float _currentMusicLevel);

        if (_currentMusicLevel == NormalMusicDB)
        {
            _audioMixer.SetFloat(MusicParam, SilentMusicDB);
            _musicEnabled = false;
        }
        else
        {
            _audioMixer.SetFloat(MusicParam, NormalMusicDB);
            _musicEnabled = true;
        }
    }

    public void ToggleSFX()
    {
        _audioMixer.GetFloat(SFXParam, out float _currentSFXLevel);

        if (_currentSFXLevel == NormalSFXDB)
        {
            _audioMixer.SetFloat(SFXParam, SilentSFXDB);
            _sfxEnabled = false;
        }
        else
        {
            _audioMixer.SetFloat(SFXParam, NormalSFXDB);
            _sfxEnabled = true;
        }
    }
}
