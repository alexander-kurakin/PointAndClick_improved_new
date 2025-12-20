using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _timerValue = 3f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private int _explosionDamage = 35;

    private MineVisual _visual;
    private float _timer;
    private bool _isTriggered = false;

    private void Awake()
    {
        _timer = _timerValue;
        _visual = GetComponent<MineVisual>();
    }

    private void Update()
    {
        if (_isTriggered)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                DetonateMine();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered == false && other.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            TriggerMine();
    }

    private void DetonateMine()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider target in targets)
        { 
            if (target.TryGetComponent<IDamageable>(out IDamageable iDamageable))
                iDamageable.TakeDamage(_explosionDamage);
        }

        _visual.AnimateDetonate();
        Destroy(gameObject);
    }

    public void TriggerMine()
    {
        _isTriggered = true;
        _timer = _timerValue;
    }

    public bool IsTriggered() => _isTriggered;
    public float GetExplosionRadius() => _explosionRadius;
}
