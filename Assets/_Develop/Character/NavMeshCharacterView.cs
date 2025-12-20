using UnityEngine;
using UnityEngine.UI;

public class NavMeshCharacterView : MonoBehaviour
{
    private const string InjuredLayerName = "Injured";
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");
    private readonly int IsDeadKey = Animator.StringToHash("IsDead");
    private readonly int TakeDamageTriggerKey = Animator.StringToHash("TakeDamage");

    [SerializeField] private float _minDistanceToFlag = 0.5f;

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshCharacter _character;
    [SerializeField] private GameObject _targetMarkerPrefab;
    [SerializeField] private Slider _slider;

    private bool _isInjuredSynchronised = false;
    private GameObject _currentTargetMarker;

    private void Update()
    {
        ShowHP();
        ShowJumping();

        if (_character.IsDead())
        {
            AnimateDeath();
            return;
        }

        if (_isInjuredSynchronised == false)
            SwitchToInjuryLayer();

        DrawMarkerAtCurrentTarget();
        DisableMarkerBasedOnProximity();

        if (_character.CurrentVelocity.magnitude> 0.05f)
            StartRunning();
        else
            StopRunning();
    }

    private void SwitchToInjuryLayer()
    {
        if (_character.IsInjured())
        {
            int injuredLayerIndex = _animator.GetLayerIndex(InjuredLayerName);
            _animator.SetLayerWeight(injuredLayerIndex, 1f);

            _isInjuredSynchronised = true;
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
    }

    private void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);
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
    }

    public void AnimateHit()
    {
        _animator.SetTrigger(TakeDamageTriggerKey);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_character.MouseHitPosition, 0.1f);
        }
    }
}
