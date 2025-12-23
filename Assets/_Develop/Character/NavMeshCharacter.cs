using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class NavMeshCharacter : MonoBehaviour, IDamageable, INavMeshMovable, IDirectionalRotatable, IHealable
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 900;
    [SerializeField] private float _rayShootDistance = 100f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _injuryThreshold = 30;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _deathTime = 3f;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private NavMeshCharacterView _view;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private AudioMixer _mixer;

    private bool _isDead = false;

    private Health _health;
    private NavMeshAgent _agent;
    private NavMeshAgentJumper _jumper;
    private AudioMixing _audioMixing;
    private NavMeshAgentMover _mover;
    private DirectionalRotator _rotator;
    private Timer _deathTimer;
    private Vector3 _targetDestination;

    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;
    public Vector3 CurrentTarget => _targetDestination;
    public Vector3 CurrentPosition => transform.position;
    public bool InJumpProcess => _jumper.InProcess;
    public bool MusicEnabled => _audioMixing.MusicEnabled;
    public bool SFXEnabled => _audioMixing.SFXEnabled;

    public bool CanMove => _isDead == false;

    public float TimeToDie => _deathTimer.TimeLimit;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.autoTraverseOffMeshLink = false;

        _mover = new NavMeshAgentMover(_agent, _moveSpeed);
        _rotator = new DirectionalRotator(transform, _rotationSpeed);
        _jumper = new NavMeshAgentJumper(_jumpSpeed, _agent, this, _jumpCurve);

        _health = new Health(_maxHealth);

        _audioMixing = new AudioMixing(_mixer);
        _audioMixing.Initialize();

        _deathTimer = new Timer(this);
    }

    private void Update()
    {
        _rotator.Update(Time.deltaTime);
    }

    public void SetDestination(Vector3 position) 
    {
        _targetDestination = position;
        _mover.SetDestination(_targetDestination);
    }

    public void ToggleMusic() => _audioMixing.ToggleMusic();
    public void ToggleSFX() => _audioMixing.ToggleSFX();

    public void StopMove() => _mover.Stop();
    public void ResumeMove() => _mover.Resume();
    public void SetRotationDirection(Vector3 inputDirection) 
        => _rotator.SetInputDirection(inputDirection);
    public bool TryGetPath (Vector3 targetPosition, NavMeshPath pathToTarget)
        => NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        _jumper.Jump(offMeshLinkData);
    }

    public bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData) 
    {
        if (_agent.isOnOffMeshLink) 
        {
            offMeshLinkData = _agent.currentOffMeshLinkData;
            return true;
        }

        offMeshLinkData = default(OffMeshLinkData);
        return false;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogError(damage);
            return;
        }

        _health.DecreaseHealth(damage);

        if (_health.HealthIsDrained)
        {
            _isDead = true;
            _deathTimer.StartProcess(_deathTime);
            return;
        }

        if (_deathTimer.InProcess(out float elapsedTime))
            return;
        
        _view.AnimateHit();
    }

    public int GetCurrentHealth() => _health.CurrentHealth;
    public bool IsDead() => _isDead;
    public bool IsInjured() => _health.CurrentHealth <= _injuryThreshold;

    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        { 
            Debug.LogError($"Heal amount {healAmount}");
            return;
        }

        if (_isDead == false)
            _health.IncreaseHealth(healAmount);
    }

    public bool InDeathProcess(out float elapsedTime) => _deathTimer.InProcess(out elapsedTime);

}
