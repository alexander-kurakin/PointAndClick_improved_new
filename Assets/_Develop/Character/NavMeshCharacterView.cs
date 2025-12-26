using System.Collections;
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

    private GameObject _currentTargetMarker;
    
    private bool _isCharacterInjured = false;
    private bool _isOnDeathEffectTriggered = false;

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshCharacter _character;
    [SerializeField] private GameObject _targetMarkerPrefab;
    [SerializeField] private GameObject _deathTextPrefab;
    [SerializeField] private Slider _slider;
    [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
    [SerializeField] private MeshRenderer _hatRenderer;
    [SerializeField] private Soundbank _soundbank;

    [SerializeField] private float _minDistanceToFlag = 0.5f;
    [SerializeField] private float _deathTextYoffset = 5f;

    private void Update()
    {
        DetectAnimLayersChange();

        ShowHP();
        
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
            StartRunning();
        else
            StopRunning();

    }

    private void SetFloatFor(SkinnedMeshRenderer bodyRenderer, MeshRenderer hatRenderer, string key, float param)
    { 
        bodyRenderer.material.SetFloat(key, param);
        hatRenderer.material.SetFloat(key, param);
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
            _soundbank.PlayInjuredSound();
        }
        else
        {
            _animator.SetLayerWeight(injuredLayerIndex, AnimLayerWeightMin);
            _soundbank.PlayHealedSound();
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
        _soundbank.SetFootstepsPlaying(false);
    }

    private void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);
        _soundbank.SetFootstepsPlaying(true);
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
            _soundbank.PlayDeadSound();

            _isOnDeathEffectTriggered = true;
        }
    }

    public void AnimateHit()
    {
        _animator.SetTrigger(TakeDamageTriggerKey);
        _soundbank.PlayHitSound();
    }

    public void DestroyVisual()
    {
        Destroy(gameObject);
    }
}
