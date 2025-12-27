using System.Collections;
using UnityEngine;

public class Soundbank : MonoBehaviour
{
    private Coroutine _currentFootstepCoroutine;

    private bool _isFootstepPlaying = false;
    private bool _isJumpSoundPlayed = false;
    private bool _isCharacterInJumpProcess = false;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _footstepsArray;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _injuredSound;
    [SerializeField] private AudioClip _healedSound;
    [SerializeField] private AudioClip _deadSound;
    [SerializeField] private float _footstepVolume = 1f;
    [SerializeField] private float _footstepRate = 0.33f;
    [SerializeField] private float _jumpVolume = 0.5f;

    private void Update()
    {
        AudioForJumps();
        AudioForFootsteps();
    }

    private void AudioForJumps()
    {
        if (_isJumpSoundPlayed == false && _isCharacterInJumpProcess)
        {
            _audioSource.volume = _jumpVolume;
            _audioSource.PlayOneShot(_jumpSound);
            _isJumpSoundPlayed = true;
        }
        else if (_isCharacterInJumpProcess == false)
        {
            _isJumpSoundPlayed = false;
        }
    }

    private void AudioForFootsteps()
    {
        if (_isCharacterInJumpProcess)
            _isFootstepPlaying = false;

        if (_isFootstepPlaying && _currentFootstepCoroutine == null)
            _currentFootstepCoroutine = StartCoroutine(ProcessFootsteps());
    }

    private void PlayRandomFootstep()
    {
        _audioSource.volume = _footstepVolume;
        _audioSource.PlayOneShot(_footstepsArray[Random.Range(0, _footstepsArray.Length)]);
    }

    private IEnumerator ProcessFootsteps()
    {
        while (_isFootstepPlaying)
        {
            PlayRandomFootstep();

            yield return new WaitForSeconds(_footstepRate);
        }

        _currentFootstepCoroutine = null;
    }
    public void SetFootstepsPlaying(bool isEnabled)
    {
        if (isEnabled)
            _isFootstepPlaying = true;
        else
            _isFootstepPlaying = false;
    }

    public void SetCharacterJumpState(bool isInJumpProcess)
    {
        _isCharacterInJumpProcess = isInJumpProcess;
    }

    public void PlayHitSound()
    {
        _audioSource.PlayOneShot(_hitSound);
    }

    public void PlayDeadSound()
    { 
        _audioSource.PlayOneShot(_deadSound);
    }

    public void PlayHealedSound()
    { 
        _audioSource.PlayOneShot(_healedSound);
    }

    public void PlayInjuredSound()
    { 
        _audioSource.PlayOneShot(_injuredSound);
    }
}


