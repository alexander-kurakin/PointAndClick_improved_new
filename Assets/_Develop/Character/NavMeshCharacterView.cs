using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavMeshCharacterView : MonoBehaviour
{
    private const string EdgeKey = "_Edge";
    private const string InjuredLayerName = "Injured";
    private const float AnimLayerWeightMax = 1f;
    private const float AnimLayerWeightMin = 0f;
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");
    private readonly int IsDeadKey = Animator.StringToHash("IsDead");
    private readonly int TakeDamageTriggerKey = Animator.StringToHash("TakeDamage");

    [SerializeField] private float _minDistanceToFlag = 0.5f;
    [SerializeField] private float _footstepRate = 0.33f;
    [SerializeField] private float _footstepVolume = 1f;
    [SerializeField] private float _jumpVolume = 0.5f;
    [SerializeField] private float _deathTextYoffset = 5f;

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshCharacter _character;
    [SerializeField] private GameObject _targetMarkerPrefab;
    [SerializeField] private GameObject _deathTextPrefab;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
    [SerializeField] private MeshRenderer _hatRenderer;

    [SerializeField] private AudioSource _heroAudioSource;
    [SerializeField] private AudioClip[] _footstepsArray;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _injuredSound;
    [SerializeField] private AudioClip _healedSound;
    [SerializeField] private AudioClip _deadSound;

    private bool _isFootstepPlaying = false;
    private bool _isJumpSoundPlayed = false;
    private bool _isCharacterInjured = false;
    private bool _isOnDeathEffectTriggered = false;

    private Coroutine _currentFootstepCoroutine;
    private GameObject _currentTargetMarker;

    private void Update()
    {
        DetectAnimLayersChange();

        ShowHP();
        ShowSoundLevels();
        ShowJumping();

        if (_character.InDeathProcess(out float elapsedTime))
            SetFloatFor(_bodyRenderer, _hatRenderer, EdgeKey, elapsedTime/_character.TimeToDie);

        if (_character.IsDead())
        {
            AnimateDeath();
            return;
        }

        DrawMarkerAtCurrentTarget();
        DisableMarkerBasedOnProximity();

        if (_character.CurrentVelocity.magnitude > 0.05f)
        {
            StartRunning();
            SoundFootsteps();
        }
        else
        {
            StopRunning();
        }

        SoundJump();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_character.MouseHitPosition, 0.1f);
        }
    }

    private void SetFloatFor(SkinnedMeshRenderer bodyRenderer, MeshRenderer hatRenderer, string key, float param)
    { 
        bodyRenderer.material.SetFloat(key, param);
        hatRenderer.material.SetFloat(key, param);
    }

    private void ShowSoundLevels()
    {
        _infoText.text = "Music : " + (_character.MusicEnabled ? "ON" : "OFF") +
            "\nSFX : " + (_character.SFXEnabled ? "ON" : "OFF");
    }

    private void DetectAnimLayersChange()
    {
        if (_character.IsInjured() != _isCharacterInjured)
            SwitchAnimLayers(_character.IsInjured());
    }

    private void SwitchAnimLayers(bool isInjured)
    {
        if (_character.IsDead())
            return;

        int injuredLayerIndex = _animator.GetLayerIndex(InjuredLayerName);
        _isCharacterInjured = isInjured;

        if (isInjured)
        {
            _animator.SetLayerWeight(injuredLayerIndex, AnimLayerWeightMax);
            _heroAudioSource.PlayOneShot(_injuredSound);
        }
        else
        {
            _animator.SetLayerWeight(injuredLayerIndex, AnimLayerWeightMin);
            _heroAudioSource.PlayOneShot(_healedSound);
        }
    }

    private void ShowHP()
    {
        float currentHP = (float) _character.GetCurrentHealth() / 100;
        _slider.value = currentHP;
    }

    private void ShowJumping()
    { 
        _animator.SetBool(InJumpProcessKey, _character.InJumpProcess);
    }

    private void StopRunning()
    {
        _animator.SetBool(IsRunningKey, false);
        _isFootstepPlaying = false;
    }

    private void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);
        _isFootstepPlaying = true;
    }

    private void SoundFootsteps()
    {
        if (_character.InJumpProcess)
            _isFootstepPlaying = false;

        if (_isFootstepPlaying && _currentFootstepCoroutine == null)
            _currentFootstepCoroutine = StartCoroutine(ProcessFootsteps());
    }

    private void SoundJump()
    {
        if (_isJumpSoundPlayed == false && _character.InJumpProcess)
        {
            _heroAudioSource.volume = _jumpVolume;
            _heroAudioSource.PlayOneShot(_jumpSound);
            _isJumpSoundPlayed = true;
        }
        else if (_character.InJumpProcess == false)
        {
            _isJumpSoundPlayed = false;
        }
    }

    private void PlayRandomFootstep()
    {
        _heroAudioSource.volume = _footstepVolume;
        _heroAudioSource.PlayOneShot(_footstepsArray[Random.Range(0, _footstepsArray.Length)]);
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

    private void DrawMarkerAtCurrentTarget()
    {
        if (_currentTargetMarker != null)
            _currentTargetMarker.transform.position = _character.CurrentTarget;
        else
            _currentTargetMarker = Instantiate(_targetMarkerPrefab, _character.CurrentTarget, Quaternion.identity);
    }

    private void DisableMarkerBasedOnProximity()
    {
        if ((_character.CurrentPosition - _currentTargetMarker.transform.position).magnitude <= _minDistanceToFlag)
            _currentTargetMarker.SetActive(false);
        else
            _currentTargetMarker.SetActive(true);
    }

    public void AnimateDeath()
    {
        _animator.SetBool(IsDeadKey, true);
        TriggerOnDeathEffects();
    }

    private void TriggerOnDeathEffects()
    { 
        if (_isOnDeathEffectTriggered == false)
        {
            Instantiate(_deathTextPrefab, transform.position + Vector3.up * _deathTextYoffset, Quaternion.identity);
            _heroAudioSource.PlayOneShot(_deadSound);

            _isOnDeathEffectTriggered = true;
        }
    }

    public void AnimateHit()
    {
        _animator.SetTrigger(TakeDamageTriggerKey);
        _heroAudioSource.PlayOneShot(_hitSound);
    }


    public void DestroyVisual()
    {
        Destroy(gameObject);
    }
}
