using UnityEngine;
using UnityEngine.AI;

public class NavMeshCharacter : MonoBehaviour, IDamageable, INavMeshMovable, IDirectionalRotatable
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotationSpeed = 900;
    [SerializeField] private float _rayShootDistance = 100f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _injuryThreshold = 30;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private NavMeshCharacterView _view;
        
    private bool _isDead = false;

    private Health _health;
    private NavMeshAgent _agent;
    private MouseRayScanner _mouseRayScanner;
    private NavMeshAgentMover _mover;
    private DirectionalRotator _rotator;
    private Vector3 _targetDestination;

    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;

    public Vector3 CurrentTarget => _targetDestination;

    public Vector3 CurrentPosition => transform.position;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new NavMeshAgentMover(_agent, _moveSpeed);
        _rotator = new DirectionalRotator(transform, _rotationSpeed);

        _health = new Health(_maxHealth);

        _mouseRayScanner = new MouseRayScanner(_rayShootDistance, _groundLayerMask);
    }

    private void Update()
    {
        _rotator.Update(Time.deltaTime);
        _mouseRayScanner.Update(Time.deltaTime);
    }

    public void SetDestination(Vector3 position) 
    {
        _targetDestination = position;
        _mover.SetDestination(_targetDestination);
    }

    public void StopMove() => _mover.Stop();
    public void ResumeMove() => _mover.Resume();
    public void SetRotationDirection(Vector3 inputDirection) 
        => _rotator.SetInputDirection(inputDirection);
    public bool TryGetPath (Vector3 targetPosition, NavMeshPath pathToTarget)
        => NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

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
            return;
        }

        _view.AnimateHit();
    }

    public int GetCurrentHealth() => _health.CurrentHealth;
    public bool IsDead() => _isDead;
    public bool IsInjured() => _health.CurrentHealth <= _injuryThreshold;

    public Vector3 MouseHitPosition => _mouseRayScanner.MouseHitPosition;

    public bool CanMove => _isDead == false;

}
